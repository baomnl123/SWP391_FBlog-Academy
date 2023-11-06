import BaseLayout from '@/components/BaseLayout'
import SiderDashboard, { FilterType } from './components/SiderDashboard'
import Card from '@/components/Card/index'
import { Button, Dropdown, Image, Modal, Spin, Typography, message } from 'antd'
import { useEffect, useState } from 'react'
import IconReport from '@/assets/images/svg/IconReport'
import RightSiderDashboard from './components/RightSiderDashboard'
import { MoreOutlined } from '@ant-design/icons'
import ModalReport from './components/ModalReport'
import ModalComment from './components/ModalComment'
import CreateUpdatePost from './components/CreateUpdatePost'
import { useNavigate } from 'react-router-dom'
import { useSelector } from 'react-redux'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import api from '@/api'
import dayjs from 'dayjs'
import ModalSave from './components/ModalSave'
import Vote from './components/Vote'
import { PendingPost } from '@/types'

export default function Dashboard() {
  const [modal, contextHolder] = Modal.useModal()
  const [openReport, setOpenReport] = useState(false)
  const [openComment, setOpenComment] = useState(false)
  const [openSave, setOpenSave] = useState(false)
  const [openPost, setOpenPost] = useState(false)
  const [idPost, setIdPost] = useState<undefined | number>(undefined)
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  const user = useSelector((state: RootState) => state.userReducer.user)
  const navigate = useNavigate()
  const [categories, setCategories] = useState<string | string[] | number | number[] | null>([])
  const [tag, setTags] = useState<string | string[] | number | number[]>([])
  const [filter, setFilter] = useState<FilterType | null>(null)
  const [postFilter, setPostFilter] = useState<PendingPost[] | null>(null)

  const { runAsync: deletePost } = useRequest(api.deletePost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Delete post success')
        getPost({})
      }
      console.log(res)
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const renderItem = (id?: number, userId?: number) => {
    return (user?.id ?? 0) === (userId ?? 1)
      ? [
          {
            key: '1',
            label: (
              <div
                onClick={() => {
                  setOpenPost(true)
                  setIdPost(id)
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
                    onOk: () => {
                      deletePost(id ?? 0)
                    }
                  })
                }}
              >
                Delete
              </div>
            )
          }
        ]
      : undefined
  }

  const { runAsync: getAllPostHasImages, loading: allPostImgsLoading } = useRequest(api.allPostHasImages, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        setPostFilter(res)
      }
      console.log(res)
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const { runAsync: getAllPostHasVideos, loading: allPostVideosLoading } = useRequest(api.allPostHasVideo, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        setPostFilter(res)
      }
      console.log(res)
    },
    onError: (err) => {
      console.log(err)
    }
  })

  useEffect(() => {
    if (!filter) {
      setPostFilter(null)
    } else if (filter === 'image') {
      getAllPostHasImages()
    } else {
      getAllPostHasVideos()
    }
  }, [filter, getAllPostHasImages, getAllPostHasVideos])

  useEffect(() => {
    getPost({
      categoryID: !categories ? undefined : (categories as unknown as number[]),
      tagID: !tag ? undefined : (tag as unknown as number[])
    })
  }, [categories, tag])

  const {
    data: postData,
    loading: postLoading,
    run: getPost
  } = useRequest(async ({ categoryID, tagID }: { categoryID?: number[]; tagID?: number[] }) => {
    try {
      const res = await api.postCategoryTag({
        categoryID,
        tagID,
        currentUserId: Number(user?.id ?? 0)
      })
      return res
    } catch (error) {
      console.log(error)
    }
  })
<<<<<<< HEAD

  useEffect(() => {
    if (!user) return
    getPost({})
  }, [user])
=======
>>>>>>> parent of f19f478 (FE Updates)

  const data = !postFilter ? postData : postFilter

  return (
    <BaseLayout
      rightSider={<RightSiderDashboard />}
      sider={
        <SiderDashboard
          onGetCategories={(data) => {
            setCategories(data)
          }}
          onGetTags={(data) => {
            setTags(data)
          }}
          createPost={() => {
            setOpenPost(true)
            setIdPost(undefined)
          }}
          onFilter={(data) => {
            setFilter(data)
          }}
        />
      }
    >
      <CreateUpdatePost
        isOpen={openPost}
        id={idPost}
        setModal={(value) => {
          setOpenPost(value)
          setIdPost(undefined)
        }}
        onFinish={() => {
          getPost({})
        }}
      />
      <Spin spinning={postLoading || allPostImgsLoading || allPostVideosLoading}>
        {data?.map((post) => {
          return (
            <div key={post?.id} className='mb-10'>
              <Card
                onClickAvatar={() => navigate(`/profile/${post?.user?.id}`)}
                user={{
                  username: post?.user?.name,
                  avatar: post?.user?.avatarUrl
                }}
                action={[
                  <div>
                    {user?.id === post?.user?.id ? (
                      <Dropdown
                        menu={{ items: renderItem(post?.id ?? 0, post?.user?.id) }}
                        placement='bottomRight'
                        key={1}
                      >
                        <Button type='text' icon={<MoreOutlined />} shape='circle' />
                      </Dropdown>
                    ) : (
                      ''
                    )}
                  </div>
                ]}
                content={post?.content}
                title={post?.title}
                createDate={dayjs(post.createdAt).format('YYYY-MM-DD')}
                slideContent={[
                  ...(post?.images ?? []).map((image) => (
                    <Image
                      key={image.id}
                      src={image.url}
                      className='w-[1194px] h-[620px]'
                      placeholder='https://i0.wp.com/thinkfirstcommunication.com/wp-content/uploads/2022/05/placeholder-1-1.png?w=1200&ssl=1'
                    />
                  )),
                  ...(post?.videos ?? []).map((video) => (
                    <video controls className='w-full'>
                      <source src={video.url} type='video/mp4' className='object-contain' />
                    </video>
                  ))
                ]}
                footer={[
                  <div key={1} className='flex items-center'>
                    <Vote
                      vote={post?.upvotes ?? 0}
                      postId={post.id}
                      userId={user?.id}
                      downvote={post.downvote}
                      upvote={post.upvote}
                      onVoteSuccess={() => {
                        getPost({})
                      }}
                    />
                  </div>,
                  <div key={2} className='cursor-pointer'>
                    <Typography
                      onClick={() => {
                        setOpenComment(true)
                        setIdPost(post?.id)
                      }}
                    >
                      Comment
                    </Typography>
                  </div>,
                  <div key={3} className='cursor-pointer'>
                    <Typography
                      onClick={() => {
                        // setOpenComment(true)
                        setIdPost(post.id)
                        setOpenSave(true)
                      }}
                    >
                      Save
                    </Typography>
                  </div>,
                  <div key={4} className='flex justify-end'>
                    <div
                      onClick={() => {
                        setOpenReport(true)
                        setIdPost(post?.id)
                      }}
                    >
                      <IconReport color={isDarkMode ? '#fff' : '#000'} />
                    </div>
                  </div>
                ]}
              ></Card>
            </div>
          )
        })}
      </Spin>
      {contextHolder}
      <ModalComment
        idPost={idPost}
        isOpen={openComment}
        setModal={(value) => {
          if (!value) {
            setIdPost(undefined)
          }
          setOpenComment(value)
        }}
      />
      <ModalReport
        idPost={idPost}
        isOpen={openReport}
        setModal={(value) => {
          if (!value) {
            setIdPost(undefined)
          }
          setOpenReport(value)
        }}
      />
      <ModalSave
        open={openSave}
        onClose={() => {
          setIdPost(undefined)
          setOpenSave(false)
        }}
        postId={idPost ?? 0}
      />
    </BaseLayout>
  )
}
