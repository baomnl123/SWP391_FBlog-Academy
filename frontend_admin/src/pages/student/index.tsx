import { Tabs, TabsProps } from 'antd'
import StudentBan from './StudentBan'
import StudentUnban from './StudentUnban'

export default function Student() {
  const items: TabsProps['items'] = [
    {
      key: '1',
      label: 'Student List',
      children: <StudentBan />
    },
    {
      key: '2',
      label: 'Banned Student List',
      children: <StudentUnban />
    }
  ]
  return <Tabs defaultActiveKey='1' items={items} />
}
