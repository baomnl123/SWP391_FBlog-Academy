import { Form, Input, Modal, ModalProps, Select, Space } from 'antd'
import { useEffect } from 'react'

const CreateTag = (
  props: ModalProps & { initialValues?: { name: string; category?: { id: number; name: string } } }
) => {
  const { open, onOk, onCancel, initialValues, ...rest } = props
  const [form] = Form.useForm()

  useEffect(() => {
    form.setFieldsValue({
      name: initialValues?.name,
      category: {
        label: initialValues?.category?.name,
        value: initialValues?.category?.id
      }
    })
  }, [form, initialValues])

  // const onFinish = useCallback(async (value) => {
  //   try {
  //     const adminId = localStorage.getItem('id') ?? ''
  //     const formData = new FormData()
  //     formData.append('tagName', value.name)
  //     formData.append('categoryId', value.category.id)
  //     formData.append('adminId', adminId)
  //     await api.createTag(formData)
  //   } catch (e) {
  //     console.error(e)
  //   }
  // }, [])

  console.log(!!initialValues?.name)

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
            <Select disabled />
          </Form.Item>
        </Space>
      </Form>
    </Modal>
  )
}

export default CreateTag
