import { Form, Input, Modal, ModalProps, Space } from 'antd'
import { useWatch } from 'antd/es/form/Form'

const CreateLecture = (
  props: ModalProps & {
    onFinish?: (value: { email: string; password: string; name: string }) => void
  }
) => {
  const { open, onOk, onCancel, ...rest } = props
  const [form] = Form.useForm()
  const name = useWatch('name', form)
  const password = useWatch('password', form)
  const email = useWatch('email', form)

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
      okButtonProps={{
        disabled: !name || !password || !email
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
          <Form.Item
            label='Email'
            name='email'
            rules={[
              { required: true, message: 'Email is required' },
              { type: 'email', message: 'Invalid E-mail!' }
            ]}
          >
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
