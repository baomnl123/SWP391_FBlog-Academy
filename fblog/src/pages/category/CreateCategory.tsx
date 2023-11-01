import { Form, Input, Modal, ModalProps } from 'antd'
import { useEffect } from 'react'

const CreateCategory = (
  props: ModalProps & { initialValues?: { name: string }; onFinish?: (value: { name: string }) => void }
) => {
  const { open, onOk, onCancel, initialValues, ...rest } = props
  const [form] = Form.useForm()

  useEffect(() => {
    form.setFieldsValue(initialValues)
  }, [form, initialValues])

  return (
    <Modal
      {...rest}
      title='Create Category'
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
    >
      <Form<{ name: string }>
        form={form}
        layout='vertical'
        onFinish={(value) => {
          props.onFinish?.(value)
          form.resetFields()
        }}
      >
        <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name category is required' }]}>
          <Input placeholder='Name category' />
        </Form.Item>
      </Form>
    </Modal>
  )
}

export default CreateCategory
