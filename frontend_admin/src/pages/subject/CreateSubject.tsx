import api from '@/config/api'
import { Form, Input, Modal, ModalProps, Space, message } from 'antd'
import { useWatch } from 'antd/es/form/Form'
import { useEffect } from 'react'

const CreateSubject = (
  props: ModalProps & {
    initialValues?: {
      subject?: {
        id: number
        name: string
      }
    }
    onSuccess?: () => void
  }
) => {
  const { open, onOk, onCancel, initialValues, onSuccess, ...rest } = props
  const [form] = Form.useForm()
  const subjectName = useWatch('name', form)

  // const { data } = useRequest(
  //   async () => {
  //     const response = await api.getCategories()
  //     return response
  //   },
  //   {
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  // const { data: detailSubject, run } = useRequest(
  //   async (id: number) => {
  //     const response = await api.getSubjectById(id)
  //     return response
  //   },
  //   {
  //     manual: true,
  //     onError(e) {
  //       console.error(e)
  //     }
  //   }
  // )

  useEffect(() => {
    form.setFieldsValue({
      name: initialValues?.subject?.name,
      major: {}
    })
    // if(initialValues?.subject?.id) {
    //   form.setFieldsValue()
    // }
  }, [form, initialValues])

  return (
    <Modal
      {...rest}
      title='Update Subject'
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
      <Form
        form={form}
        layout='vertical'
        onFinish={async (value) => {
          try {
            const adminId = localStorage.getItem('id') ?? ''
            const formData = new FormData()
            if (initialValues?.subject?.id) {
              formData.append('newSubjectName', value.name)
              await api.updateSubject(initialValues.subject.id, formData)
              message.success('Update subject successfully')
            } else {
              formData.append('subjectName', value.name)
              await api.createSubject(Number(adminId), value.major, formData)
              message.success('Create subject successfully')
            }
            onSuccess?.()
          } catch (e) {
            console.error(e)
          }
        }}
      >
        <Space className='w-full' direction='vertical' size={20}>
          <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name subject is required' }]}>
            <Input placeholder='Name subject' />
          </Form.Item>
          {/* <Form.Item
            label='Major'
            name='major'
            rules={[{ required: true, message: 'Name major is required' }]}
          >
            <Select
              allowClear
              placeholder='Select major'
              options={data?.map((option) => ({
                label: option.majorName,
                value: option.id
              }))}
            />
          </Form.Item> */}
        </Space>
      </Form>
    </Modal>
  )
}

export default CreateSubject
