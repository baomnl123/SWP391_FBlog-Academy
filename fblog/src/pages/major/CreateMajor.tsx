import api from '@/config/api'
import { Form, Input, Modal, ModalProps, message } from 'antd'
import { useWatch } from 'antd/es/form/Form'
import { useCallback, useEffect } from 'react'

const CreateMajor = (props: ModalProps & { initialValues?: { name: string; id: number }; onSuccess?: () => void }) => {
  const { open, onOk, onCancel, initialValues, onSuccess, ...rest } = props
  const [form] = Form.useForm()
  const majorName = useWatch('name', form)

  useEffect(() => {
    form.setFieldsValue(initialValues)
  }, [form, initialValues])

  const onFinish = useCallback(
    async (value: { name: string }) => {
      try {
        const payload = new FormData()
        if (initialValues) {
          if (initialValues.name !== value.name) {
            payload.append('newMajorName', value.name)
            await api.updateMajor(initialValues.id, payload)
            message.success('Update major successfully')
          }
        } else {
          const id = Number(localStorage.getItem('id'))
          payload.append('majorName', value.name)
          await api.createMajor(id, payload)
          message.success('Create major successfully')
        }
        form.resetFields()
        onSuccess?.()
      } catch (e) {
        console.error(e)
      }
    },
    [initialValues, onSuccess, form]
  )

  return (
    <Modal
      {...rest}
      title={initialValues ? 'Update Major' : 'Create Major'}
      destroyOnClose
      open={open}
      onOk={(e) => {
        form.submit()
        onOk?.(e)
      }}
      onCancel={(e) => {
        form.resetFields()
        onCancel?.(e)
      }}
      okButtonProps={{
        disabled: initialValues?.name === majorName || majorName === ''
      }}
    >
      <Form<{ name: string }> form={form} layout='vertical' onFinish={onFinish}>
        <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name major is required' }]}>
          <Input placeholder='Name major' />
        </Form.Item>
      </Form>
    </Modal>
  )
}

export default CreateMajor
