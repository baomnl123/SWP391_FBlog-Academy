import { Card, Typography } from 'antd'
import { CardProps } from 'antd/lib'
const { Title } = Typography

export type CardType = 'lecturers' | 'students' | 'post'

interface DashBoardCardProps extends CardProps {
  amount: number
  title: string
  selected?: boolean
}

const DashBoardCard = ({ amount, title, selected = false, ...props }: DashBoardCardProps) => {
  return (
    <Card
      bordered={false}
      {...props}
      className={`${selected ? 'bg-slate-50 border border-gray-100 dark:bg-slate-900 dark: dark:border-gray-800' : ''}`}
    >
      <div className='w-full text-center min-h-[140px] flex justify-center items-center cursor-pointer'>
        <Title level={1} className='mt-2 text-center'>
          {amount} {title}
        </Title>
      </div>
    </Card>
  )
}

export default DashBoardCard
