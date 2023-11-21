import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import Card from '@/components/Card'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Image, Space, Spin } from 'antd'
import dayjs from 'dayjs'
import { useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'

import SubSide from '../Dashboard/components/SubSide'

export default function UserPendingPost() {
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  const user = useSelector((state: RootState) => state.userReducer.user)

  const { data: post } = useRequest(
    async () => {
      const response = await api.getUserPendingPost(Number(user?.id ?? 0))
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

  // const { data: majors } = useRequest(
  //   async () => {
  //     const response = await api.getAllMajor()
  //     return response.map((major) => ({
  //       label: major.majorName,
  //       value: major.id
  //     }))
  //   },
  //   {
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  // const { data: subjects } = useRequest(
  //   async () => {
  //     const response = await api.getAllSubject()
  //     return response.map((subject) => ({
  //       label: subject.subjectName,
  //       value: subject.id
  //     }))
  //   },
  //   {
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  return (
    <BaseLayout sider={<SubSide />}>
      <Spin spinning={loading}>
        <div className='max-w-[1200px] mx-auto'>
          <Space size={20} className='w-full' direction='vertical'>
            {post?.map((post) => (
              <Card
                key={post?.id}
                onClickAvatar={() => navigate(`/profile/${post?.user?.id}`)}
                action={[]}
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
