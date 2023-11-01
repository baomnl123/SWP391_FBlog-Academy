import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Button, ConfigProvider, Flex, Form, Input, Modal, Space, Table, Typography, message } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import React, { useCallback, useState } from 'react'
import CreateCategory from './CreateCategory'
import ListTag from './ListTag'

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
  const [initialValues, setInitValues] = useState<{ id: number; name: string } | undefined>()
  const [category, setCategory] = useState<{ id: number; name: string } | undefined>()
  const [tag, setTag] = useState(false)
  const [form] = Form.useForm()
  const [modal, contextHolder] = Modal.useModal()

  const getTableData = async (
    { current, pageSize }: { current: number; pageSize: number },
    formData: object
  ): Promise<Result> => {
    console.log(current, pageSize, formData)
    const response = await api.getCategories()

    return Promise.resolve({
      total: response.length,
      list: response.map((item) => ({
        id: item.id,
        name: item.categoryName
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
        <Form.Item name='Search'>
          <Input.Search className='w-96' onSearch={submit} placeholder='search' />
        </Form.Item>
      </Form>
    </div>
  )

  const onDelete = useCallback(
    (e: React.MouseEvent, id: number) => {
      e.stopPropagation()
      modal.confirm({
        title: 'Delete category',
        centered: true,
        content: 'Do you want to delete this category?',
        async onOk() {
          try {
            await api.deleteCategory(id)
            message.success('Delete category success!')
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
      render: (_, record) => (
        <Space size='middle'>
          <Button
            type='text'
            onClick={(e) => {
              e.stopPropagation()
              setInitValues({
                name: record.name,
                id: record.id
              })
              setCreateCategory(true)
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
          <Button onClick={() => setCreateCategory(true)} type='primary'>
            Create category
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
              className: 'cursor-pointer',
              onClick: () => {
                setTag(true)
                setCategory({
                  name: data.name,
                  id: data.id
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
        onOk={() => {
          setCreateCategory(false)
          setInitValues(undefined)
        }}
        onSuccess={refresh}
      />
      {category && (
        <ListTag
          category={category}
          centered
          open={tag}
          onCancel={() => {
            setTag(false)
            setCategory(undefined)
          }}
        />
      )}
      {contextHolder}
    </ConfigProvider>
  )
}
