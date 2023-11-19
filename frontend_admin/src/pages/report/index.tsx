import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Table } from 'antd'
import { ColumnsType } from 'antd/es/table'
import dayjs from 'dayjs'
import { FlagOutlined } from '@ant-design/icons'

interface DataType {
  id: number
  user: string
  title: string
  createAt: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function ReportPost() {
  const getTableData = async (): Promise<Result> => {
    const response = await api.getAllPost()
    return Promise.resolve({
      total: response.length,
      list: response.map((item) => ({
        id: item.id,
        user: item.user.name,
        title: item.title,
        createAt: dayjs(item.createdAt).format('YYYY-MM-DD')
      }))
    })
  }

  const { tableProps, loading } = useAntdTable(getTableData, {
    defaultPageSize: 5
  })

  const columns: ColumnsType<DataType> = [
    {
      title: 'User',
      key: 'user',
      dataIndex: 'user'
    },
    {
      title: 'Title',
      key: 'title',
      dataIndex: 'title'
    },
    {
      title: 'Create at',
      key: 'createAt',
      dataIndex: 'createAt'
    },
    {
      title: 'Reported',
      key: 'reported',
      dataIndex: 'reported',
      render() {
        return <FlagOutlined className='text-red' />
      }
    }
  ]

  return (
    <>
      <Table
        {...tableProps}
        rowKey='id'
        columns={columns}
        loading={loading}
        onRow={() => {
          return {
            onClick() {}
          }
        }}
      />
    </>
  )
}
