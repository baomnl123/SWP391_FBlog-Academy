import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useCallback, useState } from 'react'
import CreateCategory from './CreateCategory'
import ListTag from './ListTag'
import { useAntdTable } from 'ahooks'
import { category } from '@/data'

type DataType = {
  id: number
  name: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function Category() {
  // data category
  const [cateData, setCateData] = useState(category)
  const [cateUpdate, setCateUpdate] = useState(-1)

  const [createCategory, setCreateCategory] = useState(false)
  const [initialValues, setInitValues] = useState<{ name: string } | undefined>()
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

  const searchForm = (
    <div style={{ marginBottom: 16 }}>
      <Form form={form} style={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Form.Item name='Search'>
          <Input.Search className='w-96' onSearch={submit} placeholder='search' />
        </Form.Item>
      </Form>
    </div>
  )

  const onDelete = useCallback(
    (id: number) => {
      const result = cateData.filter((cate) => cate.id !== id)
      setCateData(result)
    },
    [cateData]
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
              setCateUpdate(record.id)
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
          dataSource={cateData}
          pagination={{
            defaultPageSize: 5
          }}
          rowKey='id'
          columns={columns}
          onRow={(data) => {
            // console.log(data)
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
        onFinish={(value) => {
          if (initialValues) {
            const result = cateData.map((cate) => {
              if (cate.id === cateUpdate) {
                cate.name = value.name
              }

              return cate
            })
            setCateData(result)
          } else {
            const result = [
              {
                id: cateData.length,
                name: value.name
              },
              ...cateData
            ]
            setCateData(result)
          }
        }}
        onOk={() => {
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
