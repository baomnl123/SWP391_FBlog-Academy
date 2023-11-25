import { useCallback, useState } from 'react'
import DashBoardCard, { CardType } from './components/DashBoardCard'
import TableDashboard from './components/TableDashboard'
import { useRequest } from 'ahooks'
import api from '@/config/api'
import PostDetail from './components/PostDetail'
import { Image } from 'antd'

const Dashboard = () => {
  const [activeTab, setActiveTab] = useState<CardType | null>(null)
  const handleChangeTab = useCallback((type: CardType) => {
    setActiveTab(type)
  }, [])

  const { data: post } = useRequest(
    async () => {
      const response = await api.reportPostPending()
      return response
    },
    {
      onError(e) {
        console.error(e)
      }
    }
  )

  const { data: lecturer } = useRequest(
    async () => {
      const response = await api.getLecturers()
      return response
    },
    {
      onError(e) {
        console.error(e)
      }
    }
  )

  const { data: student } = useRequest(
    async () => {
      const response = await api.getStudent()
      return response
    },
    {
      onError(e) {
        console.error(e)
      }
    }
  )

  return (
    <>
      <div className='grid gap-6 grid-cols-3'>
        <DashBoardCard
          amount={(lecturer ?? []).length}
          title='Lecturers'
          className='w-full'
          selected={activeTab === 'lecturers'}
          onClick={() => handleChangeTab('lecturers')}
        />
        <DashBoardCard
          amount={(student ?? []).length}
          title='Students'
          className='w-full'
          selected={activeTab === 'students'}
          onClick={() => handleChangeTab('students')}
        />
        <DashBoardCard
          amount={(post ?? []).length}
          title='Reports'
          className='w-full'
          selected={activeTab === 'post'}
          onClick={() => handleChangeTab('post')}
        />
      </div>
      <div>
        {activeTab === 'post' ? (
          post?.map((item) => {
            const { id, createdAt, title, content, user, images, videos } = item.post
            return (
              <PostDetail
                key={id}
                reporterAvt={item?.reporter?.avatarUrl}
                reporterName={item?.reporter?.name}
                reportDate={item?.reporter?.createdAt}
                time={createdAt}
                reportContent={item?.content}
                upvote={item?.post.upvote}
                downvote={item?.post.downvote}
                reports={item?.post.reports}
                major={item?.post.majors.map((major) => major.majorName)}
                subject={item?.post.majors.map((subject) => subject.majorName)}
                title={title}
                description={content}
                avatar={user?.avatarUrl ?? ''}
                author={user?.name}
                className='max-w-[750px] mx-auto mt-10 mb-8'
                slideContent={[
                  ...(images ?? []).map((image) => (
                    <Image
                      key={image.id}
                      src={image.url}
                      className='w-[1194px] h-[620px]'
                      placeholder='https://i0.wp.com/thinkfirstcommunication.com/wp-content/uploads/2022/05/placeholder-1-1.png?w=1200&ssl=1'
                    />
                  )),
                  ...(videos ?? []).map((video) => (
                    <video controls className='w-full'>
                      <source src={video.url} type='video/mp4' className='object-contain' />
                    </video>
                  ))
                ]}
              />
            )
          })
        ) : (
          <TableDashboard cardType={activeTab} />
        )}
      </div>
    </>
  )
}

export default Dashboard
