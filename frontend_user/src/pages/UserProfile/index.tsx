import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card'
import { RootState } from '@/store'
import { TweenOneGroup } from 'rc-tween-one'

import { getLocalStorage } from '@/utils/helpers'
import { CheckCircleFilled, MoreOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import {
  Alert,
  Avatar,
  Button,
  Dropdown,
  Flex,
  Image,
  MenuProps,
  Modal,
  Space,
  Spin,
  Typography,
  message,
  Tag
} from 'antd'
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
import { UserMajor, UserSubject } from '@/api/types/user'

export default function UserProfile() {
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)
  const [titleModal, setTitleModal] = useState('Followers')
  const [users, setUsers] = useState<User[]>([])
  const { user } = useSelector((state: RootState) => state.userReducer)

  const [modal, contextHolder] = Modal.useModal()
  const [idPost, setIdPost] = useState<undefined | number>(undefined)
  const [openReport, setOpenReport] = useState(false)

  const [majors, setMajors] = useState<UserMajor[]>([])
  const [subjects, setSubjects] = useState<UserSubject[]>([])

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
  const { refresh } = useRequest(
    async () => {
      const response = await api.getUserMajorbyID(Number(id ?? 0))
      setMajors([...response])
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
  const { refresh: getUserSubjectbyID } = useRequest(
    async () => {
      const response = await api.getUserSubjectbyID(Number(id ?? 0))
      setSubjects([...response])
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

  // Remove Major and Subject on User's Profile
  const { runAsync: deleteUserMajor } = useRequest(api.deleteUserMajor, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Delete major success')
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const deleteMajor = (removedMajor: UserMajor) => {
    const majorList = [removedMajor.id]
    deleteUserMajor(user?.id ?? 0, majorList)

    // If there is no major, delete all subjects
    if (majors.length - 1 == 0 && subjects.length > 0) {
      const subjectID = subjects.map((subject) => subject.id)
      api.deleteUserSubject(user?.id ?? 0, subjectID ?? [])
      setSubjects([])
    }
    setMajors(majors.filter((major) => major.majorName !== removedMajor.majorName))
  }

  const mapUserMajor = (major: UserMajor) => {
    const tagElem = (
      <Tag
        closable
        onClose={(e) => {
          e.preventDefault()
          deleteMajor(major)
        }}
      >
        {major.majorName}
      </Tag>
    )
    return (
      <span key={major.id} style={{ display: 'inline-block' }}>
        {tagElem}
      </span>
    )
  }

  const { runAsync: deleteUserSubject } = useRequest(api.deleteUserSubject, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Delete subject success')
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const deleteSubject = (removedSubject: UserSubject) => {
    const subjectList = [removedSubject.id]
    deleteUserSubject(user?.id ?? 0, subjectList)
    setSubjects(subjects.filter((subject) => subject.subjectName !== removedSubject.subjectName))
  }

  const mapUserSubject = (subject: UserSubject) => {
    const tagElem = (
      <Tag
        closable
        onClose={(e) => {
          e.preventDefault()
          deleteSubject(subject)
        }}
      >
        {subject.subjectName}
      </Tag>
    )
    return (
      <span key={subject.id} style={{ display: 'inline-block' }}>
        {tagElem}
      </span>
    )
  }

  const majorChild = majors?.map(mapUserMajor)
  const subjectChild = subjects?.map(mapUserSubject)

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
                  <Typography.Text>
                    <TweenOneGroup
                      enter={{
                        scale: 0.8,
                        opacity: 0,
                        type: 'from',
                        duration: 100
                      }}
                      onEnd={(e) => {
                        if (e.type === 'appear' || e.type === 'enter') {
                          ;(e.target as any).style = 'display: inline-block'
                        }
                      }}
                      leave={{ opacity: 0, width: 0, scale: 0, duration: 200 }}
                      appear={false}
                    >
                      Major: {majorChild}
                    </TweenOneGroup>
                  </Typography.Text>
                </Flex>
                <Flex gap={100} align='center'>
                  <div
                    onClick={() => {
                      setOpenSubject(true)
                    }}
                  >
                    <UserAddOutlined color={isDarkMode ? '#fff' : '#000'} />
                  </div>
                  <Typography.Text>
                    <TweenOneGroup
                      enter={{
                        scale: 0.8,
                        opacity: 0,
                        type: 'from',
                        duration: 100
                      }}
                      onEnd={(e) => {
                        if (e.type === 'appear' || e.type === 'enter') {
                          ;(e.target as any).style = 'display: inline-block'
                        }
                      }}
                      leave={{ opacity: 0, width: 0, scale: 0, duration: 200 }}
                      appear={false}
                    >
                      Subject: {subjectChild}
                    </TweenOneGroup>
                  </Typography.Text>
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
          majorSelect={majors?.map((major) => major.id)}
          setModal={(value) => {
            if (!value) {
              setIdPost(undefined)
            }
            setOpenReport(value)
          }}
          onOk={() => {
            refresh()
            message.success('Thay đổi Major thành công! 🎉')
            setOpenReport(false)
          }}
        />

        {majors && majors.length > 0 ? (
          <ModalSubject
            idPost={idPost}
            isOpen={openSubject}
            subjectSelect={subjects?.map((subject) => subject.id)}
            setModal={(value) => setOpenSubject(value)}
            onOk={() => {
              getUserSubjectbyID()
              message.success('Thay đổi Subject thành công! 🎉')
              setOpenSubject(false)
            }}
          />
        ) : (
          <Modal visible={openSubject} onCancel={() => setOpenSubject(false)} onOk={() => setOpenSubject(false)}>
            <Alert type='warning' message='Bạn cần nhập Major trước khi nhập Subject!' />
          </Modal>
        )}
      </Spin>
      {contextHolder}
    </BaseLayout>
  )
}
