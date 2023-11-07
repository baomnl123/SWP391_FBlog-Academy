import { Card } from 'antd'

interface RightSiderDashboardMenuProps {
  title?: string
  data?: { title: string; value: string }[]
  onChange?: (value: string) => void
}
const RightSiderDashboardMenu = ({ title, data, onChange }: RightSiderDashboardMenuProps) => {
  return (
    <Card size='small' title={title ?? ''} className='w-full text-center mb-8'>
      {data?.map((item) => {
        return (
          <div className='cursor-pointer py-1' key={item.value} onClick={() => onChange?.(item.value)}>
            {item.title}
          </div>
        )
      })}
    </Card>
  )
}

const RightSiderDashboard = () => {
  return (
    <div className='px-8 mt-[80px] max-w-[300px] w-full fixed right-0'>
      <RightSiderDashboardMenu
        onChange={(value) => {
          console.log(value)
        }}
        title='Trending Category'
        data={[
          {
            title: 'Category 1',
            value: 'key-1'
          },
          {
            title: 'Category 2',
            value: 'key-2'
          },
          {
            title: 'Category 3',
            value: 'key-3'
          }
        ]}
      />
      <RightSiderDashboardMenu
        onChange={(value) => {
          console.log(value)
        }}
        title='Trending Tag'
        data={[
          {
            title: 'Tag 1',
            value: 'key-1'
          },
          {
            title: 'Tag 2',
            value: 'key-2'
          },
          {
            title: 'Tag 3',
            value: 'key-3'
          }
        ]}
      />
    </div>
  )
}

export default RightSiderDashboard
