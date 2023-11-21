import api from '@/api'
import IconReport from '@/assets/images/svg/IconReport'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card/index'
import { RootState } from '@/store'
import { PendingPost } from '@/types'
import { MoreOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Button, Dropdown, Image, Modal, Spin, Typography, message } from 'antd'
import dayjs from 'dayjs'
import { useCallback, useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import CreateUpdatePost from './components/CreateUpdatePost'
import ModalComment from './components/ModalComment'
import ModalReport from './components/ModalReport'
import ModalSave from './components/ModalSave'
import RightSiderDashboard from './components/RightSiderDashboard'
import SiderDashboard, { FilterType } from './components/SiderDashboard'
import Vote from './components/Vote'
import { getLocalStorage } from '@/utils/helpers'

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
  const [majors, setMajors] = useState<string | string[] | number | number[] | null>([])
  const [subject, setSubjects] = useState<string | string[] | number | number[]>([])
  const [filter, setFilter] = useState<FilterType | null>(null)
  const [postFilter, setPostFilter] = useState<PendingPost[] | null>(null)

  const { runAsync: deletePost } = useRequest(api.deletePost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Delete post success')
        getPost({})
      }
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

  const {
    data: postData,
    loading: postLoading,
    run: getPost,
    refresh
  } = useRequest(
    async ({ majorID, subjectID, searchValue }: { majorID?: number[]; subjectID?: number[]; searchValue?: string }) => {
      const res = await api.postMajorSubject({
        majorID,
        subjectID,
        currentUserId: Number(user?.id ?? 0),
        searchValue
      })
      return res
    },
    {
      manual: true,
      onError(e) {
        console.error(e)
      }
    }
  )

  useEffect(() => {
    if (!filter) {
      setPostFilter(null)
    } else if (filter === 'image') {
      getAllPostHasImages(user?.id ?? 0)
    } else {
      getAllPostHasVideos(user?.id ?? 0)
    }
  }, [filter, getAllPostHasImages, getAllPostHasVideos, user])

  useEffect(() => {
    if (user) {
      getPost({
        majorID: !majors ? undefined : (majors as unknown as number[]),
        subjectID: !subject ? undefined : (subject as unknown as number[])
      })
    }
  }, [majors, getPost, subject, user])

  const follow = useCallback(
    async (id: number) => {
      try {
        await api.follow(user?.id ?? getLocalStorage('id'), id)
        getPost({
          majorID: !majors ? undefined : (majors as unknown as number[]),
          subjectID: !subject ? undefined : (subject as unknown as number[])
        })
      } catch (e) {
        console.error(e)
      }
    },
    [majors, getPost, subject, user?.id]
  )

  const unFollow = useCallback(
    async (id: number) => {
      try {
        await api.unFollow(user?.id ?? getLocalStorage('id'), id)
        getPost({
          majorID: !majors ? undefined : (majors as unknown as number[]),
          subjectID: !subject ? undefined : (subject as unknown as number[])
        })
      } catch (e) {
        console.error(e)
      }
    },
    [majors, getPost, subject, user?.id]
  )
  const data = !postFilter ? postData : postFilter

  console.log(idPost)

  return (
    <BaseLayout
      showSearch
      onChangeSearch={(value) => {
        getPost({ searchValue: value })
      }}
      rightSider={
        <RightSiderDashboard
          onMajorChange={(value) => {
            setMajors(value as string | number | number[] | string[])
          }}
          onSubjectChange={(value) => {
            setSubjects(value as string | number | number[] | string[])
          }}
          onPostChange={(value) => {
            setPostFilter(value ? ([value] as unknown as PendingPost[]) : null)
          }}
        />
      }
      sider={
        <SiderDashboard
          onGetMajors={(data) => {
            setMajors(data)
          }}
          onGetSubjects={(data) => {
            setSubjects(data)
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
                onFollow={() => {
                  post.user.isFollowed ? unFollow(post.user.id) : follow(post.user.id)
                }}
                showFollow
                className='mx-auto'
                onClickAvatar={() => navigate(`/profile/${post?.user?.id}`)}
                user={{
                  username: post?.user?.name,
                  avatar: post?.user?.avatarUrl,
                  isFollow: post?.user?.isFollowed
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
                footer={
                  user?.role === 'AD'
                    ? []
                    : [
                        <div key={1} className='flex items-center'>
                          <Vote
                            vote={Number(post?.vote ?? 0)}
                            postId={post.id}
                            userId={user?.id}
                            downvote={post.downvote}
                            upvote={post.upvotes}
                            onVoteSuccess={() => {
                              refresh()
                            }}
                            // eslint-disable-next-line @typescript-eslint/no-explicit-any
                            usersVote={(post as any)?.usersUpvote}
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
                      ]
                }
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
        onClose={() => {
          setIdPost(undefined)
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
