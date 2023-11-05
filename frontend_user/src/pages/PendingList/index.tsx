import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Button, Image, Space, Spin, message } from 'antd'
import dayjs from 'dayjs'
import { useCallback, useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'

export default function PendingList() {
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()
  const reviewerId = useSelector((state: RootState) => state.userReducer.user?.id)

  const { data, refresh } = useRequest(
    async () => {
      const response = await api.postPending()
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

  const approve = useCallback(
    async (postId: number) => {
      setLoading(true)
      try {
        await api.approvePost(reviewerId ?? 0, postId)
        message.success('Approve post successfully')
        refresh()
      } catch (e) {
        console.error(e)
      } finally {
        setLoading(false)
      }
    },
    [reviewerId, refresh]
  )

  const deny = useCallback(
    async (postId: number) => {
      setLoading(true)
      try {
        await api.denyPost(reviewerId ?? 0, postId)
        message.success('Deny post successfully')
        refresh()
      } catch (e) {
        console.error(e)
      } finally {
        setLoading(false)
      }
    },
    [reviewerId, refresh]
  )

  // const { data: categories } = useRequest(
  //   async () => {
  //     const response = await api.getAllCategory()
  //     return response.map((category) => ({
  //       label: category.categoryName,
  //       value: category.id
  //     }))
  //   },
  //   {
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  // const { data: tags } = useRequest(
  //   async () => {
  //     const response = await api.getAllTag()
  //     return response.map((tag) => ({
  //       label: tag.tagName,
  //       value: tag.id
  //     }))
  //   },
  //   {
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  return (
    <BaseLayout
      sider={
        <>
          {/* <div className='mb-6'>
            <SelectLabel
              label='Tag'
              placeHolder='Select Tag'
              optionData={tags}
              onChange={(value) => {
                console.log(value)
              }}
            />
          </div>
          <div className='mb-6'>
            <SelectLabel
              label='Category'
              placeHolder='Select Category'
              optionData={categories}
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
          <Button type='primary' onClick={() => navigate('/')}>
            Dashboard
          </Button>
        </>
      }
    >
      <Spin spinning={loading}>
        <div className='max-w-[1200px] mx-auto'>
          <Space size={20} className='w-full' direction='vertical'>
            {data?.map((post) => (
              <Card
                key={post?.id}
                onClickAvatar={() => navigate(`/profile/${post?.user?.id}`)}
                action={[
                  <Button size='large' block onClick={() => approve(post?.id)}>
                    Approve
                  </Button>,
                  <Button size='large' block danger onClick={() => deny(post?.id)}>
                    Deny
                  </Button>
                ]}
                user={{
                  username: post?.user?.name,
                  avatar: post?.user?.avatarUrl
                }}
                content={post?.content}
                title={post?.title}
                createDate={dayjs().format('YYYY-MM-DD')}
                slideContent={[
                  ...(post?.images ?? []).map((image) => (
                    <Image
                      key={image?.id}
                      src={image?.url}
                      className='w-[1194px] h-[620px]'
                      placeholder='https://i0.wp.com/thinkfirstcommunication.com/wp-content/uploads/2022/05/placeholder-1-1.png?w=1200&ssl=1'
                    />
                  )),
                  ...(post?.videos ?? []).map((video) => (
                    <video controls className='w-full'>
                      <source src={video?.url} type='video/mp4' className='object-contain' />
                    </video>
                  ))
                ]}
              />
            ))}
          </Space>
        </div>
      </Spin>
    </BaseLayout>
  )
}
