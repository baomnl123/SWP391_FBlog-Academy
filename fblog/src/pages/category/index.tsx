import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useState } from 'react'
import CreateCategory from './CreateCategory'
import ListTag from './ListTag'
import { useAntdTable } from 'ahooks'

type DataType = {
  id: number
  name: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function Category() {
  const [createCategory, setCreateCategory] = useState(false)
  const [initialValues, setInitValues] = useState<{ name: string } | undefined>({
    name: ''
  })
  const [tag, setTag] = useState(false)
  const [form] = Form.useForm()
  const [modal, contextHolder] = Modal.useModal()

  const getTableData = (
    { current, pageSize }: { current: number; pageSize: number },
    formData: object
  ): Promise<Result> => {
    console.log(current, pageSize, formData)
    const data: DataType[] = []
    for (let i = 0; i < 20; i++) {
      data.push({
        id: i,
        name: `category${i}`
      })
    }
    return Promise.resolve({
      total: 20,
      list: data
    })
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
      title: 'Action',
      key: 'action',
      width: 250,
      render: (_, record) => (
        <Space size='middle'>
          <Button
            type='text'
            onClick={(e) => {
              e.stopPropagation()
              setInitValues({
                name: record.name
              })
              setCreateCategory(true)
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
                title: 'Delete category',
                centered: true,
                content: 'Do you want to delete this category?',
                onOk() {
                  console.log('ok')
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
          <Typography.Title level={5}>Quantiy: {data?.total}</Typography.Title>
          <Button onClick={() => setCreateCategory(true)} type='primary'>
            Create category
          </Button>
        </Flex>
        <Space align='start' direction='vertical' className='w-full'>
          {searchForm}
        </Space>
        <Table
          {...tableProps}
          columns={columns}
          onRow={(data) => {
            console.log(data)
            return {
              className: 'cursor-pointer',
              onClick: () => {
                setTag(true)
                setInitValues({
                  name: data.name
                })
              }
            }
          }}
        />
      </Space>
      <CreateCategory
        initialValues={initialValues}
        centered
        open={createCategory}
        onCancel={() => {
          setCreateCategory(false)
          setInitValues(undefined)
        }}
      />
      <ListTag
        category={initialValues?.name ?? ''}
        centered
        open={tag}
        onCancel={() => {
          setTag(false)
        }}
      />
      {contextHolder}
    </ConfigProvider>
  )
}
