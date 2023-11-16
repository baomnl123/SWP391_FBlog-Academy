import { useUser } from '@clerk/clerk-react'
import { Button, Card, Descriptions, DescriptionsProps, Form, Input, Modal } from 'antd'
import { useState } from 'react'

export default function Profile() {
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm()
  const { user } = useUser()

  const items: DescriptionsProps['items'] = [
    {
      key: '3',
      label: 'Name',
      children: <div>{user?.fullName ?? ''}</div>
    },
    {
      key: '1',
      label: 'Email',
      children: <div>{user?.primaryEmailAddress?.emailAddress ?? ''}</div>
    },
    {
      key: '2',
      label: 'Phone',
      children: <div>{user?.primaryPhoneNumber?.phoneNumber ?? ''}</div>
    }
  ]
  return (
    <Card>
      <Descriptions title='User Profile' items={items} />
      <Button onClick={() => setOpen(true)}>Change username</Button>
      <Modal
        open={open}
        onCancel={() => {
          setOpen(false)
          form.resetFields()
        }}
        title='Change Username'
        onOk={() => {
          form.submit()
        }}
      >
        <Form onFinish={(value) => console.log(value)} layout='vertical' form={form}>
          <Form.Item
            name='username'
            label='Username'
            initialValue={user?.fullName ?? ''}
            rules={[
              {
                required: true,
                message: 'Username is required'
              }
            ]}
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </Card>
  )
}
