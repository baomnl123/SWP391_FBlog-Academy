import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Form, Input, Modal, Space, Table, Typography, message } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useCallback, useState } from 'react'
import CreateSubject from './CreateSubject'

type DataType = {
  id: number
  name: string
  major: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function Subject() {
  // data major
  const [createSubject, setCreateSubject] = useState(false)
  const [initialValues, setInitialValues] = useState<
    | {
        subject?: {
          id: number
          name: string
        }
      }
    | undefined
  >()
  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

  const getTableData = async (_: never, { search }: { search: string }): Promise<Result> => {
    const response = await api.getAllSubject()
    const data = response.filter((item) => item.subjectName.toLowerCase().includes(search?.toLowerCase() ?? ''))
    console.log('search', search)

    return Promise.resolve({
      total: data.length,
      list: data.map((item) => ({
        id: item.id,
        name: item.subjectName,
        major: item.major.map((major) => major.majorName).join(', ')
      }))
    })
  }

  const { tableProps, search, data, refresh } = useAntdTable(getTableData, {
    defaultPageSize: 5,
    form
  })

  const onDelete = useCallback(
    async (id: number) => {
      try {
        await api.deleteSubject(id)
        message.success('Delete successfully')
        refresh()
      } catch (e) {
        console.error(e)
      }
    },
    [refresh]
  )

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
      title: 'Major',
      key: 'major',
      dataIndex: 'major'
    },
    {
      title: 'Action',
      key: 'action',
      width: 150,
      render: (_, record) => (
        <Space size='middle'>
          <Button
            type='text'
            onClick={(e) => {
              e.stopPropagation()
              setInitialValues({
                subject: {
                  id: record.id,
                  name: record.name
                }
              })
              setCreateSubject(true)
            }}
          >
            Update
          </Button>
          <Button
            type='text'
            danger
            onClick={(e) => {
              e.stopPropagation()
              modal.confirm({
                title: 'Delete subject',
                centered: true,
                content: 'Do you want to delete this subject?',
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
        <Form.Item name='search'>
          <Input.Search className='w-80' onSearch={submit} placeholder='search' />
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
        <Typography.Title level={5}>Quantity: {data?.total}</Typography.Title>
        {/* <Flex justify='space-between' align='center'>
          <Typography.Title level={5}>Quantity: {data?.total}</Typography.Title>
          <Button
            type='primary'
            onClick={() => {
              setCreateSubject(true)
            }}
          >
            Create Subject
          </Button>
        </Flex> */}
        <Space align='start' direction='vertical' className='w-full'>
          {searchForm}
        </Space>
        <Table rowKey='id' {...tableProps} columns={columns} />
      </Space>
      <CreateSubject
        initialValues={initialValues}
        centered
        open={createSubject}
        onCancel={() => {
          setCreateSubject(false)
          setInitialValues(undefined)
        }}
        onSuccess={() => {
          refresh()
          setInitialValues(undefined)
          setCreateSubject(false)
        }}
      />
      {contextHolder}
    </ConfigProvider>
  )
}
