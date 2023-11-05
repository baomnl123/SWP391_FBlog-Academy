import { Form, Input, Modal, ModalProps, Space } from 'antd'

const CreateLecture = (
  props: ModalProps & {
    onFinish?: (value: { email: string; password: string; name: string }) => void
  }
) => {
  const { open, onOk, onCancel, ...rest } = props
  const [form] = Form.useForm()

  return (
    <Modal
      {...rest}
      title='Create Lecture'
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
      <Form<{ email: string; password: string; name: string }>
        form={form}
        layout='vertical'
        onFinish={(value) => props.onFinish?.(value)}
      >
        <Space className='w-full' direction='vertical' size={20}>
          <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name is required' }]}>
            <Input placeholder='Name' />
          </Form.Item>
          <Form.Item label='Email' name='email' rules={[{ required: true, message: 'Email is required' }]}>
            <Input placeholder='Email' />
          </Form.Item>
          <Form.Item label='Password' name='password' rules={[{ required: true, message: 'Password is required' }]}>
            <Input.Password placeholder='Password' />
          </Form.Item>
        </Space>
      </Form>
    </Modal>
  )
}

export default CreateLecture
