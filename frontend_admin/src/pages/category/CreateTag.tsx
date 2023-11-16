import api from '@/config/api'
import { Form, Input, Modal, ModalProps, Select, Space, message } from 'antd'
import { useWatch } from 'antd/es/form/Form'
import { useCallback, useEffect } from 'react'

const CreateTag = (
  props: ModalProps & {
    initialValues?: { tag?: { name: string; id: number }; category?: { id: number; name: string } }
    onSuccess?: () => void
  }
) => {
  const { open, onOk, onCancel, initialValues, onSuccess, ...rest } = props
  const [form] = Form.useForm()
  const tagName = useWatch('name', form)

  useEffect(() => {
    form.setFieldsValue({
      name: initialValues?.tag?.name,
      category: {
        label: initialValues?.category?.name,
        value: initialValues?.category?.id
      }
    })
  }, [form, initialValues])

  const onFinish = useCallback(
    async (value: {
      name: string
      category: {
        value: number
      }
    }) => {
      try {
        const adminId = localStorage.getItem('id') ?? ''
        const formData = new FormData()
        if (initialValues?.tag?.name) {
          formData.append('newTagName', value.name)
          await api.updateTag(initialValues.tag?.id ?? 0, formData)
          message.success('Update tag successfully')
        } else {
          formData.append('tagName', value.name)
          await api.createTag(Number(adminId), value.category.value, formData)
          message.success('Create tag successfully')
        }

        onSuccess?.()
      } catch (e) {
        console.error(e)
      }
    },
    [initialValues?.tag?.id, initialValues?.tag?.name, onSuccess]
  )

  return (
    <Modal
      {...rest}
      title={initialValues?.tag?.id ? 'Update Tag' : 'Create Tag'}
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
        disabled: initialValues?.tag?.name === tagName || tagName === ''
      }}
    >
      <Form form={form} layout='vertical' onFinish={onFinish}>
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
