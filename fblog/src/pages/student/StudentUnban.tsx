import api from '@/config/api'
import { UserEmail } from '@/types'
import { useAntdTable, useRequest } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography, message } from 'antd'
import { ColumnsType } from 'antd/lib/table'

type DataType = {
  id: number
  name: string
  email: string
}

type Result = {
  total: number
  list: UserEmail[]
}

export default function StudentUnban() {
  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

  const { runAsync: getStudent } = useRequest(api.getStudentUnban, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        return res
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const { runAsync: unbanStudent } = useRequest(api.unbanStudent, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Unban student success')
        submit()
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const getTableData = async (_: never, { search }: { search: string }): Promise<Result> => {
    const res = await getStudent()
    if (res) {
      let object = {
        total: res.length,
        list: res
      }
      if (search) {
        const data = res.filter((item) => item.name.includes(search))
        object = {
          total: data.length,
          list: data
        }
      }

      return object
    }
    return {
      total: 0,
      list: []
    }
  }

  const { tableProps, search, data } = useAntdTable(getTableData, {
    defaultPageSize: 5,
    form
  })

  const { submit } = search

  const columns: ColumnsType<DataType> = [
    {
      title: 'ID',
      key: 'id',
      dataIndex: 'id'
    },
    {
      title: 'Name',
      key: 'name',
      dataIndex: 'name'
    },
    {
      title: 'Email',
      key: 'email',
      dataIndex: 'email'
    },
    {
      title: 'Action',
      key: 'action',
      width: 150,
      render: (_, record) => (
        <Space size='middle'>
          <Button
            type='text'
            danger
            onClick={(e) => {
              e.stopPropagation()
              modal.confirm({
                title: 'Unban student',
                centered: true,
                content: 'Are you sure to unban student?',
                onOk() {
                  unbanStudent(record.id)
                },
                onCancel() {
                  console.log('cancel')
                }
              })
            }}
          >
            Unban
          </Button>
        </Space>
      )
    }
  ]

  const searchForm = (
    <div style={{ marginBottom: 16 }}>
      <Form form={form} style={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Form.Item name='search'>
          <Input.Search className='w-96' onSearch={submit} placeholder='search' />
        </Form.Item>
      </Form>
    </div>
  )

  return (
    <ConfigProvider
      theme={{
        components: {
          Form: {
            itemMarginBottom: 0
          }
        }
      }}
    >
      <Space className='w-full' size={20} direction='vertical'>
        <Flex justify='space-between' align='center'>
          <Typography.Title level={5}>Account: {data?.total}</Typography.Title>
        </Flex>
        <div>
          <Space align='start' direction='vertical' className='w-full'>
            {searchForm}
          </Space>
          <Table {...tableProps} columns={columns} />
        </div>
      </Space>
      {contextHolder}
    </ConfigProvider>
  )
}
