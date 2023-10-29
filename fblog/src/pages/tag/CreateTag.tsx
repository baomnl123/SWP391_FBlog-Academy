import { Form, Input, Modal, ModalProps, Select, SelectProps, Space } from 'antd'
import { useEffect } from 'react'

const CreateTag = (props: ModalProps & { initialValues?: { name: string; category: string[] } }) => {
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
      title='Create Tag'
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
          <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name tag is required' }]}>
            <Input placeholder='Name tag' />
          </Form.Item>
          <Form.Item
            label='Category'
            name='category'
            rules={[{ required: true, message: 'Name category is required' }]}
          >
            <Select mode='multiple' allowClear placeholder='Select category' options={options} />
          </Form.Item>
        </Space>
      </Form>
    </Modal>
  )
}

export default CreateTag
