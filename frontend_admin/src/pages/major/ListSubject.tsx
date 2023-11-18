import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, ModalProps, Space, Table, Typography, message } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useCallback, useState } from 'react'
import CreateSubject from './CreateSubject'

type DataType = {
  id: number
  name: string
}

type Result = {
  total: number
  list: DataType[]
}

const ListSubject = (props: ModalProps & { major?: { id: number; name: string } }) => {
  const { open, onOk, onCancel, title, major, ...rest } = props
  const [form] = Form.useForm()
  const [createSubject, setCreateSubject] = useState(false)
  const [initialValues, setInitialValues] = useState<
    { subject?: { name: string; id: number }; major?: { id: number; name: string } } | undefined
  >()
  const [modal, contextHolder] = Modal.useModal()

  const getTableData = async (
    { current, pageSize }: { current: number; pageSize: number },
    formData: object
  ): Promise<Result> => {
    console.log(current, pageSize, formData)
    const response = await api.getSubjectByMajor(major?.id ?? 0)
    return Promise.resolve({
      total: response?.length,
      list: (response ?? []).map((item) => ({
        id: item.id,
        name: item.subjectName
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
        await api.deleteSubjectFromMajor(major?.id ?? 0, id)
        message.success('Delete subject successfully')
      } catch (e) {
        console.error(e)
      }
    },
    [major?.id]
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
                },
                major
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
                async onOk() {
                  await onDelete(record.id)
                  refresh()
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
        title={title ?? 'Subject list'}
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
                setCreateSubject(true)
                setInitialValues({
                  subject: {
                    name: '',
                    id: 0
                  },
                  major
                })
              }}
            >
              Create Subject
            </Button>
          </Flex>
          <div>
            <Space align='start' direction='vertical' className='w-full'>
              {searchForm}
            </Space>
            <Table {...tableProps} columns={columns} rowKey='id' />
          </div>
        </Space>
      </Modal>
      <CreateSubject
        initialValues={initialValues}
        centered
        open={createSubject}
        onCancel={() => setCreateSubject(false)}
        onOk={() => {
          setCreateSubject(false)
        }}
        onSuccess={() => {
          refresh()
        }}
      />
      {contextHolder}
    </ConfigProvider>
  )
}

export default ListSubject
