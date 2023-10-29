import { Form, Input, Modal, ModalProps, SelectProps, Space } from 'antd'
import { useEffect } from 'react'

const CreateLecture = (props: ModalProps & { initialValues?: { name: string; category: string[] } }) => {
  const { open, onOk, onCancel, initialValues, ...rest } = props
  const [form] = Form.useForm()
  console.log(initialValues)
  useEffect(() => {
    form.setFieldsValue({
      name: initialValues?.name ?? '',
      category: initialValues?.category ?? []
    })
  }, [form, initialValues])

  const options: SelectProps['options'] = []
  for (let i = 0; i < 20; i++) {
    options.push({
      label: `category ${i}`,
      value: `category ${i}`
    })
  }

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
      <Form<{ name: string }> form={form} layout='vertical' onFinish={(value) => console.log(value)}>
        <Space className='w-full' direction='vertical' size={20}>
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
