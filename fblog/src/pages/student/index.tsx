import { student } from '@/data'
import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useState } from 'react'

type DataType = {
  id: number
  name: string
}

type Result = {
  total: number
  list: DataType[]
}

const getTableData = (
  { current, pageSize }: { current: number; pageSize: number },
  formData: object
): Promise<Result> => {
  console.log(current, pageSize, formData)
  const data: DataType[] = []
  for (let i = 0; i < 20; i++) {
    data.push({
      id: i,
      name: `Tag${i}`
    })
  }
  return Promise.resolve({
    total: 20,
    list: data
  })
}

export default function Student() {
  // student data
  const [studentData, setStudentData] = useState(student)

  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

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
                title: 'Ban student',
                centered: true,
                content: 'Are you sure to ban student?',
                onOk() {
                  const result = studentData.filter((student) => student.id !== record.id)
                  setStudentData(result)
                },
                onCancel() {
                  console.log('cancel')
                }
              })
            }}
          >
            Ban
          </Button>
        </Space>
      )
    }
  ]

  const searchForm = (
    <div style={{ marginBottom: 16 }}>
      <Form form={form} style={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Form.Item name='Search'>
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
          <Typography.Title level={5}>Account: {studentData?.length}</Typography.Title>
        </Flex>
        <div>
          <Space align='start' direction='vertical' className='w-full'>
            {searchForm}
          </Space>
          <Table
            {...tableProps}
            dataSource={studentData}
            pagination={{
              defaultPageSize: 5
            }}
            columns={columns}
          />
        </div>
      </Space>
      {contextHolder}
    </ConfigProvider>
  )
}
