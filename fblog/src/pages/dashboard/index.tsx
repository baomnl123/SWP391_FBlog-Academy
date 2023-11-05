import { useCallback, useState } from 'react'
import DashBoardCard, { CardType } from './components/DashBoardCard'
import TableDashboard from './components/TableDashboard'
import { useRequest } from 'ahooks'
import api from '@/config/api'

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
          title='Post'
          className='w-full'
          selected={activeTab === 'post'}
          onClick={() => handleChangeTab('post')}
        />
      </div>
      <div>
        <TableDashboard cardType={activeTab} />
      </div>
    </>
  )
}

export default Dashboard
