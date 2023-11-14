import api from '@/config/api'
import { Lecturers } from '@/types'
import { useAntdTable, useRequest } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography, message } from 'antd'
import { ColumnsType } from 'antd/es/table'
import { useCallback, useState } from 'react'
import CreateLecture from './components/CreateLecture'

type DataType = {
  id: number
  name: string
  email: string
}

type Result = {
  total: number
  list: Lecturers[]
}

export default function Lecture() {
  // data lecturer

  const [createLecture, setCreateLecture] = useState(false)
  const [modal, contextHolder] = Modal.useModal()
  const [form] = Form.useForm()

  const { runAsync: getLectures } = useRequest(api.getLecturers, {
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

  const getTableData = async (_: never, { search }: { search: string }): Promise<Result> => {
    const res = await getLectures()
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

  const { runAsync: createLectures } = useRequest(
    async (value) => {
      const res = await api.createLectures(value)
      return res
    },
    {
      manual: true,
      onSuccess: (res) => {
        if (res) {
          message.success('Create lectures success')
          submit()
          setCreateLecture(false)
        }
      },
      onError: (err) => {
        console.log(err)
      }
    }
  )

  const { runAsync: deleteLectures } = useRequest(api.deleteLectures, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Delete lectures success')
        submit()
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const onDelete = useCallback((id: number) => {
    deleteLectures(id.toString())
  }, [])

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
          <Typography.Title level={5}>Account: {data?.total ?? 0}</Typography.Title>
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
        <Table {...tableProps} columns={columns} />
      </Space>
      <CreateLecture
        centered
        open={createLecture}
        onCancel={() => setCreateLecture(false)}
        onFinish={(value) => {
          createLectures(value)
        }}
        // onOk={() => setCreateLecture(false)}
      />
      {contextHolder}
    </ConfigProvider>
  )
}
