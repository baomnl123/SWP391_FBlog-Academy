import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card'
import { CheckCircleFilled, MoreOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Avatar, Button, Dropdown, Flex, Image, MenuProps, Modal, Spin, Typography, message } from 'antd'
import dayjs from 'dayjs'
import { useCallback, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import CreateUpdatePost from '../Dashboard/components/CreateUpdatePost'
import { useSelector } from 'react-redux'
import { RootState } from '@/store'

export default function UserProfile() {
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)
  const [idPost, setIdPost] = useState<number | undefined>()
  const [modal, contextHolder] = Modal.useModal()
  const navigate = useNavigate()
  const { id } = useParams()
  const currentId = useSelector<RootState>((state) => state.userReducer.user?.id)

  // const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  // const [filter, setFilter] = useState('')

  const { data: userInfo } = useRequest(
    async () => {
      const response = await api.getUserById(Number(id ?? 0))
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

  const { data: follower } = useRequest(
    async () => {
      const response = await api.followerByUserId(Number(id ?? 0))
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

  const { data: following } = useRequest(
    async () => {
      const response = ''
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
    <BaseLayout
      sider={
        <>
          {/* <div className='mb-6'>
            <SelectLabel
              label='Tag'
              placeHolder='Select Tag'
              optionData={optionsTag}
              onChange={(value) => {
                console.log(value)
              }}
            />
          </div>
          <div className='mb-6'>
            <SelectLabel
              label='Category'
              placeHolder='Select Category'
              optionData={optionsCategory}
              onChange={(value) => {
                console.log(value)
              }}
            />
          </div>
          <Space
            className={`mt-8 w-full cursor-pointer ${filter === 'image' ? 'bg-blue-600' : ''} py-2 px-10 rounded-md`}
            size={10}
            onClick={() => {
              setFilter('image')
            }}
          >
            <IconPicture color={isDarkMode ? '#fff' : '#000'} width={30} height={30} />
            <Typography.Text>Image</Typography.Text>
          </Space>
          <Space
            className={`mt-8 w-full cursor-pointer ${filter === 'video' ? 'bg-blue-600' : ''} py-2 px-10 rounded-md`}
            size={10}
            onClick={() => {
              setFilter('video')
            }}
          >
            <IconPhotoFilm color={isDarkMode ? '#fff' : '#000'} width={30} height={30} />
            <Typography.Text>Video</Typography.Text>
          </Space> */}
          <Button block type='primary' onClick={() => navigate('/')}>
            Dashboard
          </Button>
        </>
      }
    >
      <Spin spinning={loading}>
        <div className='max-w-[1200px] mx-auto'>
          <div className='flex gap-5 mb-10  '>
            <Avatar className='flex-none' size={128} src={userInfo?.avatarUrl} />
            <div className='grow'>
              <Flex vertical justify='space-around' align='start' className='h-full'>
                <Typography.Title level={3}>
                  {userInfo?.name} {userInfo?.isAwarded && <CheckCircleFilled className='w-[20px] text-blue-600' />}
                </Typography.Title>
                <Flex gap={100} align='center'>
                  <Typography.Text>{posts?.length ?? 0} Posts</Typography.Text>
                  <Typography.Text>{follower?.length ?? 0} Followers</Typography.Text>
                  <Typography.Text>{following?.length ?? 0} Following</Typography.Text>
                </Flex>
              </Flex>
            </div>
          </div>
          {posts?.map((post) => (
            <Card
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
        <CreateUpdatePost
          isOpen={open}
          id={idPost}
          setModal={(value) => {
            setOpen(value)
            setIdPost(undefined)
          }}
        />
      </Spin>

      {contextHolder}
    </BaseLayout>
  )
}
