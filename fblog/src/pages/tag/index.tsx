import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useState, useCallback } from 'react'
import CreateTag from './CreateTag'
import { tag } from '@/data'

type DataType = {
  id: number
  name: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function Tag() {
  // data category
  const [tagData, setTagData] = useState(tag)
  const [tagUpdate, setTagUpdate] = useState(-1)

  const [createTag, setCreateTag] = useState(false)
  const [initialValues, setInitialValues] = useState<{ name: string; category: string[] } | undefined>()
  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

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

  const onDelete = useCallback(
    (id: number) => {
      const result = tagData.filter((cate) => cate.id !== id)
      setTagData(result)
    },
    [tagData]
  )

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
            onClick={(e) => {
              e.stopPropagation()
              setInitialValues({
                name: record.name,
                category: []
              })
              setCreateTag(true)
              setTagUpdate(record.id)
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
        <Form.Item name='Search'>
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
        <Flex justify='space-between' align='center'>
          <Typography.Title level={5}>Quantity: {tagData?.length}</Typography.Title>
          <Button
            type='primary'
            onClick={() => {
              setCreateTag(true)
            }}
          >
            Create Tag
          </Button>
        </Flex>
        <Space align='start' direction='vertical' className='w-full'>
          {searchForm}
        </Space>
        <Table rowKey='id' {...tableProps} columns={columns} dataSource={tagData} pagination={{ defaultPageSize: 5 }} />
      </Space>
      <CreateTag
        initialValues={initialValues}
        centered
        open={createTag}
        onCancel={() => {
          setCreateTag(false)
          setInitialValues(undefined)
        }}
        onFinish={(value) => {
          console.log('onFinish')
          if (initialValues) {
            const result = tagData.map((tag) => {
              if (tag.id === tagUpdate) {
                tag.name = value.name
              }

              return tag
            })
            setTagData(result)
          } else {
            const result = [
              {
                id: tagData.length,
                name: value.name
              },
              ...tagData
            ]
            setTagData(result)
          }
        }}
        onOk={() => {
          setCreateTag(false)
          setInitialValues(undefined)
        }}
      />
      {contextHolder}
    </ConfigProvider>
  )
}
