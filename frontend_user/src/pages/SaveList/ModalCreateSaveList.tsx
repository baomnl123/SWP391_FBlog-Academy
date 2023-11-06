import api from '@/api'
import { RootState } from '@/store'
import { Form, Input, Modal, message } from 'antd'
import { useCallback, useEffect } from 'react'
import { useSelector } from 'react-redux'

type IModalCreateSaveList = {
  open: boolean
  onClose: () => void
  onOk?: () => void
  saveList?: {
    id: number
    name: string
  }
}

export default function ModalCreateSaveList({ open, onClose, onOk, saveList }: IModalCreateSaveList) {
  const [form] = Form.useForm()
  const id = useSelector<RootState>((state) => state.userReducer.user?.id)
  const createSaveList = useCallback(
    async (value: { name: string }) => {
      try {
        if (id) {
          const formData = new FormData()
          formData.append('name', value.name)
          if (saveList?.id) {
            await api.updateSaveList(saveList.id, formData)
            message.success('Update save list success')
          } else {
            await api.createSaveList(id as number, formData)
            message.success('Create save list success')
          }
        }
      } catch (e) {
        console.error(e)
      }
    },
    [id, saveList?.id]
  )

  useEffect(() => {
    form.setFieldValue('name', saveList?.name)
  }, [form, saveList?.name])

  return (
    <Modal
      open={open}
      onCancel={() => {
        onClose()
        form.resetFields()
      }}
      title={`${saveList?.id ? 'Update' : 'Create'} Save List`}
      okText={`${saveList?.id ? 'Update' : 'Create'}`}
      onOk={() => {
        form.submit()
      }}
      centered
    >
      <Form<{ name: string }>
        initialValues={{
          name: saveList?.name
        }}
        form={form}
        layout='vertical'
        onFinish={async (value) => {
          console.log(value)
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
