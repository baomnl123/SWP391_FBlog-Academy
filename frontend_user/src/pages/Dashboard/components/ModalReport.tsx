import api from '@/api'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Form, Modal, Spin, Typography, message } from 'antd'
import TextArea from 'antd/es/input/TextArea'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'

interface ModalReportProps {
  isOpen: boolean
  setModal?: (value: boolean) => void
  idPost?: number | null
  onSuccess?: () => void
}

const ModalReport = ({ isOpen, setModal, idPost, onSuccess }: ModalReportProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)

  const { runAsync: sendReport, loading: reportLoading } = useRequest(api.reportPost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Send report success')
        onSuccess?.()
        form.resetFields()
        setIsModalOpen(false)
        setModal?.(false)
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  useEffect(() => {
    setIsModalOpen(isOpen)
  }, [isOpen])

  const handleOk = () => {
    form.submit()
  }

  const handleCancel = () => {
    form.resetFields()
    onSuccess?.()
    setIsModalOpen(false)
    setModal?.(false)
  }

  const onFinish = async (value: { content: string }) => {
    await sendReport({
      content: value.content,
      postID: idPost ?? 0,
      reporterID: user?.id ?? 0
    })
  }

  return (
    <Spin spinning={reportLoading}>
      <Modal open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
        <Typography.Title level={3} className='text-center mt-3'>
          Report
        </Typography.Title>
        <Form form={form} onFinish={onFinish}>
          {/* <Form.Item name='title' rules={[{ required: true, message: 'Please input your title!' }]}>
          <Input placeholder='Title' />
        </Form.Item> */}
          <Form.Item name='content' rules={[{ required: true, message: 'Please input your content!' }]}>
            <TextArea placeholder='Content' rows={5} />
          </Form.Item>
        </Form>
      </Modal>
    </Spin>
  )
}

export default ModalReport
