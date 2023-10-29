import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, ModalProps, Space, Table, Typography } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useState } from 'react'
import CreateTag from './CreateTag'

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

const ListTag = (props: ModalProps & { category: string }) => {
  const { open, onOk, onCancel, title, category, ...rest } = props
  const [form] = Form.useForm()
  const [createTag, setCreateTag] = useState(false)
  const [initialValues, setInitialValues] = useState<{ name: string; category: string } | undefined>({
    name: '',
    category: ''
  })
  const [modal, contextHolder] = Modal.useModal()

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
                category: category ?? ''
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
      <Modal
        {...rest}
        title={title ?? 'Tag list'}
        open={open}
        onOk={onOk}
        onCancel={onCancel}
        footer={false}
        width={1500}
      >
        <Space className='w-full' size={20} direction='vertical'>
          <Flex justify='space-between' align='center'>
            <Typography.Title level={5}>Quantity: {data?.total}</Typography.Title>
            <Button
              type='primary'
              onClick={() => {
                setCreateTag(true)
                setInitialValues({
                  name: '',
                  category
                })
              }}
            >
              Create Tag
            </Button>
          </Flex>
          <div>
            <Space align='start' direction='vertical' className='w-full'>
              {searchForm}
            </Space>
            <Table {...tableProps} columns={columns} />
          </div>
        </Space>
      </Modal>
      <CreateTag initialValues={initialValues} centered open={createTag} onCancel={() => setCreateTag(false)} />
      {contextHolder}
    </ConfigProvider>
  )
}

export default ListTag
