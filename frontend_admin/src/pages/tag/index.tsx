import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Form, Input, Modal, Space, Table, Typography, message } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useCallback, useState } from 'react'
import CreateTag from './CreateTag'

type DataType = {
  id: number
  name: string
  category: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function Tag() {
  // data category
  const [createTag, setCreateTag] = useState(false)
  const [initialValues, setInitialValues] = useState<
    | {
        tag?: {
          id: number
          name: string
        }
      }
    | undefined
  >()
  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

  const getTableData = async (_: never, { search }: { search: string }): Promise<Result> => {
    const response = await api.getTags()
    const data = response.filter((item) => item.tagName.toLowerCase().includes(search?.toLowerCase() ?? ''))
    console.log('search', search)

    return Promise.resolve({
      total: data.length,
      list: data.map((item) => ({
        id: item.id,
        name: item.tagName,
        category: item.categories.map((category) => category.categoryName).join(', ')
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
        await api.deleteTag(id)
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
      title: 'Category',
      key: 'category',
      dataIndex: 'category'
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
                tag: {
                  id: record.id,
                  name: record.name
                }
              })
              setCreateTag(true)
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
              setCreateTag(true)
            }}
          >
            Create Tag
          </Button>
        </Flex> */}
        <Space align='start' direction='vertical' className='w-full'>
          {searchForm}
        </Space>
        <Table rowKey='id' {...tableProps} columns={columns} />
      </Space>
      <CreateTag
        initialValues={initialValues}
        centered
        open={createTag}
        onCancel={() => {
          setCreateTag(false)
          setInitialValues(undefined)
        }}
        onOk={() => {
          setCreateTag(false)
        }}
        onSuccess={() => {
          refresh()
          setInitialValues(undefined)
        }}
      />
      {contextHolder}
    </ConfigProvider>
  )
}
