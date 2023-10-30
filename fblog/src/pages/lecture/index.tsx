import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useState, useCallback } from 'react'
import CreateLecture from './components/CreateLecture'
import { lecturer } from '@/data'

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
      name: `Lecturer${i}`
    })
  }
  return Promise.resolve({
    total: 20,
    list: data
  })
}

export default function Lecture() {
  // data lecturer
  const [dataLec, setDataLec] = useState(lecturer)

  const [createLecture, setCreateLecture] = useState(false)
  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

  const { tableProps, search, data } = useAntdTable(getTableData, {
    defaultPageSize: 5,
    form
  })

  const { submit } = search

  const onDelete = useCallback(
    (id: number) => {
      const result = dataLec.filter((lec) => lec.id !== id)
      setDataLec(result)
    },
    [dataLec]
  )

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
                title: 'Delete tag',
                centered: true,
                content: 'Do you want to delete this tag?',
                onOk() {
                  onDelete(record.id)
                },
                onCancel() {
                  console.log('cancel')
                }
              })
            }}
          >
            Delete
          </Button>
        </Space>
      )
    }
  ]

  const searchForm = (
    <div style={{ marginBottom: 16 }}>
      <Form form={form} style={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Form.Item name='Search'>
          <Input.Search className='w-80' onSearch={submit} placeholder='Search' />
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
          <Button
            type='primary'
            onClick={() => {
              setCreateLecture(true)
            }}
          >
            Create Lecturer
          </Button>
        </Flex>
        <Space align='start' direction='vertical' className='w-full'>
          {searchForm}
        </Space>
        <Table {...tableProps} dataSource={dataLec} pagination={{ defaultPageSize: 5 }} columns={columns} />
      </Space>
      <CreateLecture
        centered
        open={createLecture}
        onCancel={() => setCreateLecture(false)}
        onFinish={(value) => {
          const result = [
            {
              id: lecturer.length,
              name: value.email
            },
            ...lecturer
          ]
          setDataLec(result)
        }}
        onOk={() => setCreateLecture(false)}
      />
      {contextHolder}
    </ConfigProvider>
  )
}
