import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography, message } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import React, { useCallback, useState } from 'react'
import CreateMajor from './CreateMajor'
import ListSubject from './ListSubject'

type DataType = {
  id: number
  name: string
}

type Result = {
  total: number
  list: DataType[]
}

export default function Major() {
  const [createMajor, setCreateMajor] = useState(false)
  const [initialValues, setInitValues] = useState<{ id: number; name: string } | undefined>()
  const [major, setMajor] = useState<{ id: number; name: string } | undefined>()
  const [subject, setSubject] = useState(false)
  const [form] = Form.useForm()
  const [modal, contextHolder] = Modal.useModal()

  const getTableData = async (_: never, { search }: { search: string }): Promise<Result> => {
    const response = await api.getAllMajor()
    const data = response.filter((item) => item.majorName.toLowerCase().includes(search?.toLowerCase() ?? ''))
    return Promise.resolve({
      total: data.length,
      list: data.map((item) => ({
        id: item.id,
        name: item.majorName
      }))
    })
  }

  const { tableProps, search, data, refresh, loading } = useAntdTable(getTableData, {
    defaultPageSize: 5,
    form
  })

  const { submit } = search

  const searchForm = (
    <div style={{ marginBottom: 16 }}>
      <Form form={form} style={{ display: 'flex', justifyContent: 'flex-end' }}>
        <Form.Item name='search'>
          <Input.Search className='w-96' onSearch={submit} placeholder='search' />
        </Form.Item>
      </Form>
    </div>
  )

  const onDelete = useCallback(
    (e: React.MouseEvent, id: number) => {
      e.stopPropagation()
      modal.confirm({
        title: 'Delete major',
        centered: true,
        content: 'Do you want to delete this major?',
        async onOk() {
          try {
            await api.deleteMajor(id)
            message.success('Delete major success!')
            refresh()
          } catch (e) {
            console.error(e)
          }
        },
        onCancel() {
          console.log('cancel')
        }
      })
    },
    [modal, refresh]
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
      render: (_, record) =>
        record.name !== 'Only Students' && (
          <Space size='middle'>
            <Button
              type='text'
              onClick={(e) => {
                e.stopPropagation()
                setInitValues({
                  name: record.name,
                  id: record.id
                })
                setCreateMajor(true)
              }}
            >
              Update
            </Button>
            <Button type='text' danger onClick={(e) => onDelete(e, record.id)}>
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
          <Button onClick={() => setCreateMajor(true)} type='primary'>
            Create major
          </Button>
        </Flex>
        <Space align='start' direction='vertical' className='w-full'>
          {searchForm}
        </Space>
        <Table
          {...tableProps}
          loading={loading}
          rowKey='id'
          columns={columns}
          onRow={(data) => {
            return {
              className: data.name !== 'Only Students' ? 'cursor-pointer' : '',
              onClick: () => {
                if (data.name !== 'Only Students') {
                  setSubject(true)
                  setMajor({
                    name: data.name,
                    id: data.id
                  })
                }
              }
            }
          }}
        />
      </Space>
      <CreateMajor
        initialValues={initialValues}
        centered
        open={createMajor}
        onCancel={() => {
          setCreateMajor(false)
          setInitValues(undefined)
        }}
        onSuccess={() => {
          setCreateMajor(false)
          setInitValues(undefined)
          refresh()
        }}
      />
      {major && (
        <ListSubject
          major={major}
          centered
          open={subject}
          onCancel={() => {
            setSubject(false)
            setMajor(undefined)
          }}
        />
      )}
      {contextHolder}
    </ConfigProvider>
  )
}
