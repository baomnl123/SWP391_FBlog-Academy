import { Form, Table } from 'antd'
import { CardType } from './DashBoardCard'
import { useAntdTable, useRequest } from 'ahooks'
import { useEffect } from 'react'
import api from '@/config/api'

interface TableDashboardProps {
  cardType: CardType | null
}

interface Result {
  total: number
  list: {
    id: number
    name: string
  }[]
}

const TableDashboard = ({ cardType }: TableDashboardProps) => {
  const [form] = Form.useForm()

  const { runAsync: getPost } = useRequest(
    async () => {
      const response = await api.reportPostPending()
      return response
    },
    {
      manual: true,
      onError(e) {
        console.error(e)
      }
    }
  )

  const { runAsync: getLecturers } = useRequest(
    async () => {
      const response = await api.getLecturers()
      return response
    },
    {
      manual: true,
      onError(e) {
        console.error(e)
      }
    }
  )

  const { runAsync: getStudent } = useRequest(
    async () => {
      const response = await api.getStudent()
      return response
    },
    {
      manual: true,
      onError(e) {
        console.error(e)
      }
    }
  )

  const getTableData = async (): Promise<Result> => {
    if (!cardType) return Promise.reject()
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    let data: { id: number; name: string }[] = []
    if (cardType === 'lecturers') {
      const response = await getLecturers()
      data = response?.map((item) => ({
        id: item?.id,
        name: item?.name,
        email: item?.email
      }))
    }
    if (cardType === 'students') {
      const response = await getStudent()
      data = response?.map((item) => ({
        id: item?.id,
        name: item?.name,
        email: item?.email
      }))
    }
    if (cardType === 'post') {
      const response = await getPost()
      data = response.map((item) => ({
        id: item?.post?.id,
        name: item?.post?.title,
        Reporter: item?.reporter?.name
      }))
    }

    return {
      total: data.length,
      list: data
    }
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

  const columns = [
    {
      title: 'ID',
      dataIndex: 'id'
    },
    {
      title: 'Name',
      dataIndex: 'name'
    },
    ...(cardType === 'post' ? [{ title: 'Reporter', dataIndex: 'Reporter' }] : [{ title: 'Email', dataIndex: 'email' }])
  ]

  if (!cardType) return null
  return (
    <div className='mt-10'>
      <Table
        columns={columns}
        rowKey='id'
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
