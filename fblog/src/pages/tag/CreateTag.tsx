import api from '@/config/api'
import { Form, Input, Modal, ModalProps, Space, message } from 'antd'
import { useEffect } from 'react'

const CreateTag = (
  props: ModalProps & {
    initialValues?: {
      tag?: {
        id: number
        name: string
      }
    }
    onSuccess?: () => void
  }
) => {
  const { open, onOk, onCancel, initialValues, onSuccess, ...rest } = props
  const [form] = Form.useForm()

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

  // const { data: detailTag, run } = useRequest(
  //   async (id: number) => {
  //     const response = await api.getTagById(id)
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
      name: initialValues?.tag?.name,
      category: {}
    })
    // if(initialValues?.tag?.id) {
    //   form.setFieldsValue()
    // }
  }, [form, initialValues])

  return (
    <Modal
      {...rest}
      title='Update Tag'
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
      <Form
        form={form}
        layout='vertical'
        onFinish={async (value) => {
          try {
            console.log(value)
            const adminId = localStorage.getItem('id') ?? ''
            const formData = new FormData()
            if (initialValues?.tag?.id) {
              formData.append('newTagName', value.name)
              await api.updateTag(initialValues.tag.id, formData)
              message.success('Update tag successfully')
            } else {
              formData.append('tagName', value.name)
              await api.createTag(Number(adminId), value.category, formData)
              message.success('Create tag successfully')
            }
            onSuccess?.()
          } catch (e) {
            console.error(e)
          }
        }}
      >
        <Space className='w-full' direction='vertical' size={20}>
          <Form.Item label='Name' name='name' rules={[{ required: true, message: 'Name tag is required' }]}>
            <Input placeholder='Name tag' />
          </Form.Item>
          {/* <Form.Item
            label='Category'
            name='category'
            rules={[{ required: true, message: 'Name category is required' }]}
          >
            <Select
              allowClear
              placeholder='Select category'
              options={data?.map((option) => ({
                label: option.categoryName,
                value: option.id
              }))}
            />
          </Form.Item> */}
        </Space>
      </Form>
    </Modal>
  )
}

export default CreateTag
