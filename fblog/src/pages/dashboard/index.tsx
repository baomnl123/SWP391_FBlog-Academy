import { useCallback, useState } from 'react'
import DashBoardCard, { CardType } from './components/DashBoardCard'
import TableDashboard from './components/TableDashboard'

const Dashboard = () => {
  const [activeTab, setActiveTab] = useState<CardType | null>(null)
  const handleChangeTab = useCallback((type: CardType) => {
    setActiveTab(type)
  }, [])
  return (
    <>
      <div className='grid gap-6 grid-cols-3'>
        <DashBoardCard
          amount={20}
          title='Lecturers'
          className='w-full'
          selected={activeTab === 'lecturers'}
          onClick={() => handleChangeTab('lecturers')}
        />
        <DashBoardCard
          amount={20}
          title='Students'
          className='w-full'
          selected={activeTab === 'students'}
          onClick={() => handleChangeTab('students')}
        />
        <DashBoardCard
          amount={20}
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
