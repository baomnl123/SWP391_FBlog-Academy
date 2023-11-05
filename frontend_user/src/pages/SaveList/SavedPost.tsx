import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card'
import { useRequest } from 'ahooks'
import { Button, Image, Space, Typography, message } from 'antd'
import dayjs from 'dayjs'
import { useCallback } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

export default function SavedPost() {
  const { id } = useParams()
  const navigate = useNavigate()

  const { data, refresh } = useRequest(
    async () => {
      const response = await api.postSaved(Number(id))
      return response
    },
    {
      onError(e) {
        console.error(e)
      }
    }
  )

  const unsave = useCallback(
    async (postID: number) => {
      try {
        await api.deletePostFromSaveList(Number(id), postID)
        message.success('Unsave success')
        refresh()
      } catch (e) {
        console.error(e)
      }
    },
    [id, refresh]
  )

  return (
    <BaseLayout
      sider={
        <div>
          <Button onClick={() => navigate('/save-list')} block type='primary'>
            Save List
          </Button>
        </div>
      }
    >
      <Space className='w-full' direction='vertical' size={50}>
        {data?.map((post) => (
          <Card
            user={{
              username: post?.user?.name,
              avatar: post?.user?.avatarUrl
            }}
            title={post?.title}
            content={post?.content}
            action={[]}
            key={post?.id}
            createDate={dayjs(post?.createdAt).format('YYYY-MM-DD')}
            onClickAvatar={() => navigate(`/profile/${post?.user?.id}`)}
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
            footer={[
              <div key={3} className='cursor-pointer'>
                <Typography
                  onClick={() => {
                    unsave(post?.id)
                  }}
                >
                  Unsave
                </Typography>
              </div>
            ]}
          />
        ))}
      </Space>
    </BaseLayout>
  )
}
