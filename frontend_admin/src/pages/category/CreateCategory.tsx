import api from '@/config/api'
import { Form, Input, Modal, ModalProps, message } from 'antd'
import { useWatch } from 'antd/es/form/Form'
import { useCallback, useEffect } from 'react'

const CreateCategory = (
  props: ModalProps & { initialValues?: { name: string; id: number }; onSuccess?: () => void }
) => {
  const { open, onOk, onCancel, initialValues, onSuccess, ...rest } = props
  const [form] = Form.useForm()
  const categoryName = useWatch('name', form)

  useEffect(() => {
    form.setFieldsValue(initialValues)
  }, [form, initialValues])

  const onFinish = useCallback(
    async (value: { name: string }) => {
      try {
        const payload = new FormData()
        if (initialValues) {
          if (initialValues.name !== value.name) {
            payload.append('newCategoryName', value.name)
            await api.updateCategory(initialValues.id, payload)
            message.success('Update category successfully')
          }
        } else {
          const id = Number(localStorage.getItem('id'))
          payload.append('categoryName', value.name)
          await api.createCategory(id, payload)
          message.success('Create category successfully')
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
      title={initialValues ? 'Update Category' : 'Create Category'}
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
        disabled: initialValues?.name === categoryName || categoryName === ''
      }}
    >
      <Form<{ name: string }> form={form} layout='vertical' onFinish={onFinish}>
        <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name category is required' }]}>
          <Input placeholder='Name category' />
        </Form.Item>
      </Form>
    </Modal>
  )
}

export default CreateCategory
