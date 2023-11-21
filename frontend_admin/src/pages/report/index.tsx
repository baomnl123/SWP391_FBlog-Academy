import api from '@/config/api'
import { useAntdTable } from 'ahooks'
import { Table } from 'antd'
import { ColumnsType } from 'antd/es/table'
import dayjs from 'dayjs'
import { FlagOutlined } from '@ant-design/icons'
import { useState } from 'react'
import ModalPost from './ModalPost'
// import ModalReportPost from './ModalReportPost'
import { Post } from '@/types'

interface DataType {
  id: number
  user: string
  title: string
  createAt: string
  reported: number
  majors: string[]
  subjects: string[]
}

type Result = {
  total: number
  list: DataType[]
}

export default function ReportPost() {
  const [showModal, setShowModal] = useState(false)
  // const [showModalReport, setShowModalReport] = useState(false)
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
        reported: item.reports,
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
      dataIndex: 'subjects',
      sorter: (a, b) => {
        return a.subjects[0].length - b.subjects[0].length
      }
    },
    {
      title: 'Majors',
      key: 'majors',
      dataIndex: 'majors',
      sorter: (a, b) => {
        return a.majors[0].length - b.majors[0].length
      }
    },
    {
      title: 'Reported',
      key: 'reported',
      dataIndex: 'reported',
      render(value) {
        return (
          value > 0 && (
            <>
              <span className='mr-2 text-red-600'>{value}</span>
              <FlagOutlined className='text-red-600' />
            </>
          )
        )
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
        onRow={(data) => {
          return {
            onClick() {
              setIndexPost(data.id ?? 0)
              setShowModal(true)
              // if (!data.reported) {
              //   setShowModal(true)
              // } else {
              //   setShowModalReport(true)
              // }
            }
          }
        }}
      />
      <ModalPost
        open={showModal}
        onCancel={() => {
          setShowModal(false)
          setIndexPost(-1)
        }}
        footer={false}
        data={(post ?? {}).find((x) => x.id === indexPost) as Post}
        onBan={() => {
          setShowModal(false)
          refresh()
        }}
      />
      {/* <ModalReportPost
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
      /> */}
    </>
  )
}
