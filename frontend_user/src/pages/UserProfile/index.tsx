import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card'
import { RootState } from '@/store'

import { getLocalStorage } from '@/utils/helpers'
import { CheckCircleFilled, MoreOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Avatar, Button, Dropdown, Flex, Image, MenuProps, Modal, Space, Spin, Typography, message } from 'antd'
import dayjs from 'dayjs'
import { useCallback, useState } from 'react'
import { useSelector } from 'react-redux'
import { useParams } from 'react-router-dom'
import ModalListUsers from './components/ModalListUsers'
import { User } from '@/types'
import SubSide from '../Dashboard/components/SubSide'
import { UserAddOutlined } from '@ant-design/icons'

import ModalMajor from './components/ModalMajor'
import ModalSubject from './components/ModalSubject'

export default function UserProfile() {
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)
  const [titleModal, setTitleModal] = useState('Followers')
  const [users, setUsers] = useState<User[]>([])
  const [modal, contextHolder] = Modal.useModal()
  const [idPost, setIdPost] = useState<undefined | number>(undefined)
  const [openReport, setOpenReport] = useState(false)
  const [openSubject, setOpenSubject] = useState(false)
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)

  // const navigate = useNavigate()
  const { id } = useParams()
  const currentId = useSelector<RootState>((state) => state.userReducer.user?.id)

  // const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  // const [filter, setFilter] = useState('')

  const { data: userInfo, refresh: getUserById } = useRequest(
    async () => {
      const response = await api.getUserById(Number(id ?? 0))
      const follow = await runAsyncFollower()
      const isFollowed = !!follow.find((user) => user.id === Number(currentId ?? getLocalStorage('id')))
      return { ...response, isFollowed }
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )
  const { data: userMajor, refresh } = useRequest(
    async () => {
      const response = await api.getUserMajorbyID(Number(id ?? 0))
      return response
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )
  const { data: userSubject } = useRequest(
    async () => {
      const response = await api.getUserSubjectbyID(Number(id ?? 0))
      return response
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )

  const { data: posts } = useRequest(
    async () => {
      const response = await api.getPostByUserId(Number(id ?? 0))
      return response
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )

  const {
    data: follower,
    refresh: getFollower,
    runAsync: runAsyncFollower
  } = useRequest(
    async () => {
      const response = await api.followerByUserId(Number(currentId ?? getLocalStorage('id')), Number(id ?? 0))
      setUsers(response)
      return response
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )

  const {
    data: following,
    refresh: getFollowing,
    runAsync: runAsyncFollowing
  } = useRequest(
    async () => {
      const response = await api.followingByUserId(Number(currentId ?? getLocalStorage('id')), Number(id ?? 0))
      setUsers(response)
      return response
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )

  // const { data: isFollow, refresh: checkIsFollow } = useRequest(
  //   async () => {
  //     const response = await api.followingByUserId(Number(currentId ?? getLocalStorage('id')))
  //     if (response.find((user) => user.id === Number(id))) {
  //       return true
  //     }
  //     return false
  //   },
  //   {
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  // const optionsTag: SelectProps['options'] = [
  //   {
  //     label: 'Option 1',
  //     value: 1
  //   },
  //   {
  //     label: 'Option 2',
  //     value: 2
  //   },
  //   {
  //     label: 'Option 3',
  //     value: 3
  //   }
  // ]

  // const optionsCategory: SelectProps['options'] = [
  //   {
  //     label: 'Category 1',
  //     value: 1
  //   },
  //   {
  //     label: 'Category 2',
  //     value: 2
  //   },
  //   {
  //     label: 'Category 3',
  //     value: 3
  //   }
  // ]

  const onDelete = useCallback(async (id: number) => {
    try {
      await api.deletePost(id)
      message.success('Delete successfully')
    } catch (error) {
      console.error(error)
    }
  }, [])

  const follow = useCallback(async () => {
    try {
      await api.follow(currentId ?? getLocalStorage('id'), Number(id))
      getUserById()
      getFollower()
    } catch (e) {
      console.error(e)
    }
  }, [currentId, id, getUserById, getFollower])

  const unFollow = useCallback(async () => {
    try {
      await api.unFollow(currentId ?? getLocalStorage('id'), Number(id))
      getUserById()
      getFollower()
    } catch (e) {
      console.error(e)
    }
  }, [currentId, id, getUserById, getFollower])

  const items: MenuProps['items'] = [
    {
      key: '1',
      label: (
        <div
          onClick={() => {
            setOpen(true)
          }}
        >
          Update
        </div>
      )
    },
    {
      key: '2',
      label: (
        <div
          className='cursor-pointer w-20 text-red-500'
          onClick={() => {
            modal.warning({
              title: 'Warning',
              content: 'Are you sure to delete?',
              closable: true,
              okText: 'Delete',
              okButtonProps: {
                danger: true,
                type: 'primary'
              },
              async onOk() {
                await onDelete(idPost ?? 0)
              }
            })
          }}
        >
          Delete
        </div>
      )
    }
  ]

  return (
    <BaseLayout sider={<SubSide />}>
      <Spin spinning={loading}>
        <div className='max-w-[1200px] mx-auto'>
          <div className='flex gap-5 mb-10  '>
            <Avatar className='flex-none' size={128} src={userInfo?.avatarUrl} />
            <div className='grow'>
              <Flex vertical justify='space-around' align='start' className='h-full'>
                <Space size={10} align='center'>
                  <Typography.Text className='text-2xl font-semibold'>{userInfo?.name}</Typography.Text>
                  {userInfo?.isAwarded && <CheckCircleFilled className='w-[20px] text-blue-600' />}
                  {Number(currentId ?? getLocalStorage('id')) !== Number(id) && (
                    <Typography.Link
                      className='text-lg flex justify-center items-center'
                      onClick={async () => {
                        userInfo?.isFollowed ? unFollow() : follow()
                      }}
                    >
                      {userInfo?.isFollowed ? 'Unfollow' : 'Follow'}
                    </Typography.Link>
                  )}
                </Space>

                <Flex gap={100} align='center'>
                  <Typography.Text>{posts?.length ?? 0} Posts</Typography.Text>
                  <Typography.Text
                    className='cursor-pointer'
                    onClick={async () => {
                      setOpen(true)
                      setTitleModal('Followers')
                      const response = await runAsyncFollower()
                      setUsers(response ?? [])
                    }}
                  >
                    {follower?.length ?? 0} Followers
                  </Typography.Text>
                  <Typography.Text
                    className='cursor-pointer'
                    onClick={async () => {
                      setOpen(true)
                      setTitleModal('Following')
                      const response = await runAsyncFollowing()
                      setUsers(response ?? [])
                    }}
                  >
                    {following?.length ?? 0} Following
                  </Typography.Text>
                </Flex>
                <Flex gap={100} align='center'>
                  <div
                    onClick={() => {
                      setOpenReport(true)
                    }}
                  >
                    <UserAddOutlined color={isDarkMode ? '#fff' : '#000'} />
                  </div>
                  <Typography.Text>Major : {userMajor?.map((item) => item.majorName)}</Typography.Text>
                </Flex>
                <Flex gap={100} align='center'>
                  <div
                    onClick={() => {
                      setOpenSubject(true)
                    }}
                  >
                    <UserAddOutlined color={isDarkMode ? '#fff' : '#000'} />
                  </div>
                  <Typography.Text>Subject : {userSubject?.map((item) => item.subjectName)}</Typography.Text>
                </Flex>
              </Flex>
            </div>
          </div>

          {posts?.map((post) => (
            <Card
              className='mb-5'
              key={post.id}
              action={
                id === currentId
                  ? [
                      <Dropdown menu={{ items }} placement='bottomRight'>
                        <Button type='text' icon={<MoreOutlined />} shape='circle' onClick={() => setIdPost(post.id)} />
                      </Dropdown>
                    ]
                  : []
              }
              user={{
                username: post.user.name,
                avatar: post.user.avatarUrl
              }}
              content={post.content}
              title={post.title}
              createDate={dayjs(post.createdAt).format('YYYY-MM-DD')}
              slideContent={[
                ...(post.images ?? []).map((image) => (
                  <Image
                    key={image.id}
                    src={image.url}
                    className='w-[1194px] h-[620px]'
                    placeholder='https://i0.wp.com/thinkfirstcommunication.com/wp-content/uploads/2022/05/placeholder-1-1.png?w=1200&ssl=1'
                  />
                )),
                ...(post.videos ?? []).map((video) => (
                  <video controls className='w-full'>
                    <source src={video.url} type='video/mp4' className='object-contain' />
                  </video>
                ))
              ]}
            />
          ))}
        </div>
        <ModalListUsers
          open={open}
          title={<p className='text-center'>{titleModal}</p>}
          onCancel={() => {
            setOpen(false)
          }}
          footer={false}
          users={users}
          onToggleFollow={() => {
            titleModal === 'Followers' ? getFollower() : getFollowing()
          }}
        />
        <ModalMajor
          idPost={idPost}
          isOpen={openReport}
          majorSelect={userMajor?.map((major) => major.id)}
          setModal={(value) => {
            if (!value) {
              setIdPost(undefined)
            }
            setOpenReport(value)
          }}
          onOk={() => {
            refresh()
          }}
        />
        <ModalSubject
          idPost={idPost}
          isOpen={openSubject}
          subjectSelect={userSubject?.map((subject) => subject.id)}
          setModal={(value) => {
            if (!value) {
              setIdPost(undefined)
            }
            setOpenSubject(value)
          }}
          onOk={() => {
            refresh()
          }}
        />
      </Spin>
      {contextHolder}
    </BaseLayout>
  )
}
