import { Form, Input, Modal, ModalProps } from 'antd'
import { useEffect } from 'react'

const CreateCategory = (props: ModalProps & { initialValues?: { name: string } }) => {
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
      <Form<{ name: string }> form={form} layout='vertical' onFinish={(value) => console.log(value)}>
        <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name category is required' }]}>
          <Input placeholder='Name category' />
        </Form.Item>
      </Form>
    </Modal>
  )
}

export default CreateCategory
