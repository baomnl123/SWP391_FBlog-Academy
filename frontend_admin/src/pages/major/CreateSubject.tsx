import api from '@/config/api'
import { Form, Input, Modal, ModalProps, Select, Space, message } from 'antd'
import { useWatch } from 'antd/es/form/Form'
import { useCallback, useEffect } from 'react'

const CreateSubject = (
  props: ModalProps & {
    initialValues?: { subject?: { name: string; id: number }; major?: { id: number; name: string } }
    onSuccess?: () => void
  }
) => {
  const { open, onOk, onCancel, initialValues, onSuccess, ...rest } = props
  const [form] = Form.useForm()
  const subjectName = useWatch('name', form)

  useEffect(() => {
    form.setFieldsValue({
      name: initialValues?.subject?.name,
      major: {
        label: initialValues?.major?.name,
        value: initialValues?.major?.id
      }
    })
  }, [form, initialValues])

  const onFinish = useCallback(
    async (value: {
      name: string
      major: {
        value: number
      }
    }) => {
      try {
        const adminId = localStorage.getItem('id') ?? ''
        const formData = new FormData()
        if (initialValues?.subject?.name) {
          formData.append('newSubjectName', value.name)
          await api.updateSubject(initialValues.subject?.id ?? 0, formData)
          message.success('Update subject successfully')
        } else {
          formData.append('subjectName', value.name)
          await api.createSubject(Number(adminId), value.major.value, formData)
          message.success('Create subject successfully')
        }

        onSuccess?.()
      } catch (e) {
        console.error(e)
      }
    },
    [initialValues?.subject?.id, initialValues?.subject?.name, onSuccess]
  )

  return (
    <Modal
      {...rest}
      title={initialValues?.subject?.id ? 'Update Subject' : 'Create Subject'}
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
        disabled: initialValues?.subject?.name === subjectName || subjectName === ''
      }}
    >
      <Form form={form} layout='vertical' onFinish={onFinish}>
        <Space className='w-full' direction='vertical' size={20}>
          <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name subject is required' }]}>
            <Input placeholder='Name subject' />
          </Form.Item>
          <Form.Item label='Category' name='major' rules={[{ required: true, message: 'Name major is required' }]}>
            <Select disabled />
          </Form.Item>
        </Space>
      </Form>
    </Modal>
  )
}

export default CreateSubject
