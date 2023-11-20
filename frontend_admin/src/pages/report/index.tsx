import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Table } from 'antd'
import { ColumnsType } from 'antd/es/table'
import dayjs from 'dayjs'
import { FlagOutlined } from '@ant-design/icons'
import { useState } from 'react'
import ModalPost from './ModalPost'
import ModalReportPost from './ModalReportPost'
import { Post } from '@/types'

interface DataType {
  id: number
  user: string
  title: string
  createAt: string
  reported: boolean
  majors: string[]
  subjects: string[]
}

type Result = {
  total: number
  list: DataType[]
}

export default function ReportPost() {
  const [showModal, setShowModal] = useState(false)
  const [showModalReport, setShowModalReport] = useState(false)
  const [indexPost, setIndexPost] = useState(-1)
  const [post, setPost] = useState<Post[]>([])

  const getTableData = async (): Promise<Result> => {
    const response = await api.getAllPost()
    setPost(response)
    return Promise.resolve({
      total: response.length,
      list: response.map((item) => ({
        id: item.id,
        user: item.user.name,
        title: item.title,
        createAt: dayjs(item.createdAt).format('YYYY-MM-DD'),
        reported: item.reports > 0,
        majors: item.majors.map((major) => major.majorName),
        subjects: item.subjects.map((subject) => subject.subjectName)
      }))
    })
  }

  const { tableProps, loading, refresh } = useAntdTable(getTableData, {
    defaultPageSize: 5
  })

  const columns: ColumnsType<DataType> = [
    {
      title: 'User',
      key: 'user',
      dataIndex: 'user'
    },
    {
      title: 'Title',
      key: 'title',
      dataIndex: 'title'
    },
    {
      title: 'Create at',
      key: 'createAt',
      dataIndex: 'createAt'
    },
    {
      title: 'Subjects',
      key: 'subjects',
      dataIndex: 'subjects'
    },
    {
      title: 'Majors',
      key: 'majors',
      dataIndex: 'majors'
    },
    {
      title: 'Reported',
      key: 'reported',
      dataIndex: 'reported',
      render(value) {
        return value && <FlagOutlined className='text-red-600' />
      }
    }
  ]

  return (
    <>
      <Table
        {...tableProps}
        rowKey='id'
        columns={columns}
        loading={loading}
        onRow={(data, index) => {
          return {
            onClick() {
              setIndexPost(index ?? 0)
              if (!data.reported) {
                setShowModal(true)
              } else {
                setShowModalReport(true)
              }
            }
          }
        }}
      />
      <ModalPost
        open={showModal}
        onCancel={() => {
          setShowModal(false)
        }}
        footer={false}
        data={post[indexPost]}
      />
      <ModalReportPost
        open={showModalReport}
        onCancel={() => {
          setShowModalReport(false)
        }}
        data={post[indexPost]}
        footer={false}
        onSuccess={() => {
          setShowModalReport(false)
          refresh()
        }}
      />
    </>
  )
}
