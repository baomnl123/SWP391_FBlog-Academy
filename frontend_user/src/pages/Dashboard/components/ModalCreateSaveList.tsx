import api from '@/api'
import { RootState } from '@/store'
import { Form, Input, Modal, message } from 'antd'
import { useCallback } from 'react'
import { useSelector } from 'react-redux'

type IModalCreateSaveList = {
  open: boolean
  onClose: () => void
  onOk?: () => void
}

export default function ModalCreateSaveList({ open, onClose, onOk }: IModalCreateSaveList) {
  const [form] = Form.useForm()
  const id = useSelector<RootState>((state) => state.userReducer.user?.id)
  const createSaveList = useCallback(
    async (value: { name: string }) => {
      try {
        if (id) {
          const formData = new FormData()
          formData.append('name', value.name)
          await api.createSaveList(id as number, formData)
          message.success('Create save list success')
        }
      } catch (e) {
        console.error(e)
      }
    },
    [id]
  )
  return (
    <Modal
      open={open}
      onCancel={onClose}
      title='Create Save List'
      okText='Create'
      onOk={() => {
        form.submit()
        onClose?.()
      }}
    >
      <Form<{ name: string }>
        form={form}
        layout='vertical'
        onFinish={async (value) => {
          await createSaveList(value)
          form.resetFields()
          onOk?.()
        }}
      >
        <Form.Item name='name' label='Name' rules={[{ required: true }]}>
          <Input placeholder='Name save list' />
        </Form.Item>
      </Form>
    </Modal>
  )
}
