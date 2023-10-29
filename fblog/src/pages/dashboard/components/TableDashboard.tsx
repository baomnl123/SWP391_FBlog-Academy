import { Form, Table } from 'antd'
import { CardType } from './DashBoardCard'
import { useAntdTable } from 'ahooks'
import { useEffect } from 'react'

interface TableDashboardProps {
  cardType: CardType | null
}

interface Item {
  name: {
    last: string
  }
  email: string
  phone: string
  gender: 'male' | 'female'
}

interface Result {
  total: number
  list: Item[]
}

interface FilterData {
  search?: string
  type?: CardType | null
}

const TableDashboard = ({ cardType }: TableDashboardProps) => {
  const [form] = Form.useForm()
  const getTableData = (
    { current, pageSize }: { current: number; pageSize: number },
    formData: FilterData
  ): Promise<Result> => {
    if (!cardType) return Promise.reject()
    let query = `page=${current}&size=${pageSize}`
    Object.entries(formData).forEach(([key, value]) => {
      if (value) {
        query += `&${key}=${value}`
      }
    })

    return fetch(`https://randomuser.me/api?results=55&${query}`)
      .then((res) => res.json())
      .then((res) => ({
        total: res.info.results,
        list: res.results
      }))
  }
  const { tableProps, search } = useAntdTable(getTableData, {
    defaultPageSize: 5,
    form
  })
  const { submit } = search

  useEffect(() => {
    if (!cardType) return
    submit?.()
  }, [cardType, submit])

  const columns =
    cardType !== 'post'
      ? [
          {
            title: 'ID',
            dataIndex: 'id',
            render: () => {
              return 'ID1'
            }
          },
          {
            title: 'Name',
            dataIndex: 'name',
            render: () => {
              return 'Name of data'
            }
          },
          {
            title: 'Role',
            dataIndex: 'role',
            render: () => {
              return 'Role of data'
            }
          }
        ]
      : [
          {
            title: 'ID',
            dataIndex: 'id',
            render: () => {
              return 'ID1'
            }
          },
          {
            title: 'Name',
            dataIndex: 'name',
            render: () => {
              return 'Name of data'
            }
          }
        ]

  if (!cardType) return null
  return (
    <div className='mt-10'>
      <Table
        columns={columns}
        {...tableProps}
        pagination={{
          pageSizeOptions: [],
          showSizeChanger: false,
          defaultPageSize: 5
        }}
      />
    </div>
  )
}

export default TableDashboard
