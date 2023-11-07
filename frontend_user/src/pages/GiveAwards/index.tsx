import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import { PlusOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Avatar, Button, Card, Flex, Space, Spin, Typography, message } from 'antd'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CreateUpdatePost from '../Dashboard/components/CreateUpdatePost'

export default function GiveAwards() {
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()
  const [open, setOpen] = useState(false)

  // const options: SelectProps['options'] = [
  //   {
  //     label: 'Option 1',
  //     value: 1
  //   },
  //   {
  //     label: 'Option 2',
  //     value: 2
  //   },
  //   {
  //     label: 'Option 3',
  //     value: 3
  //   }
  // ]

  const { data, refresh } = useRequest(
    async () => {
      const response = await api.getStudentAndModerator()
      return response
    },
    {
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      },
      onError(e) {
        console.error(e)
      }
    }
  )

  const { run: giveAward } = useRequest(
    async (userId: number) => {
      await api.giveAward(userId)
      refresh()
    },
    {
      manual: true,
      onSuccess() {
        message.success('give award successfully')
      },
      onError(e) {
        console.error(e)
      },
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      }
    }
  )

  const { run: removeAward } = useRequest(
    async (userId: number) => {
      await api.removeAward(userId)
      refresh()
    },
    {
      manual: true,
      onSuccess() {
        message.success('remove award successfully')
      },
      onError(e) {
        console.error(e)
      },
      onBefore() {
        setLoading(true)
      },
      onFinally() {
        setLoading(false)
      }
    }
  )

  return (
    <BaseLayout
      sider={
        <Flex justify='space-between' align='center' vertical className='h-full w-full'>
          <div className='w-full'>
            {/* <SelectLabel
              placeHolder='Sorted By'
              optionData={options}
              onChange={(value) => {
                console.log(value)
              }}
              mode={undefined}
              className='w-full'
            />
            <Button block type='primary' onClick={() => navigate('/')}>
              Dashboard
            </Button> */}
          </div>
          <Button size='large' block type='primary' onClick={() => setOpen(true)}>
            <Flex justify='space-between' align='center'>
              <Typography.Text className='text-white'>Create Post</Typography.Text>
              <PlusOutlined />
            </Flex>
          </Button>
        </Flex>
      }
    >
      <Spin spinning={loading}>
        <div className='w-full'>
          <Card className='max-w-[800px] mx-auto'>
            <Space className='w-full' size={20} direction='vertical'>
              {data?.map((user) => (
                <Flex
                  justify='space-between'
                  align='center'
                  className='cursor-pointer'
                  onClick={() => navigate(`/profile/${user?.id}`)}
                >
                  <Space size={10}>
                    <Avatar size={64} src={user.avatarUrl} />
                    <Typography.Text>{user.name}</Typography.Text>
                  </Space>
                  {user.isAwarded ? (
                    <Button
                      type='primary'
                      danger
                      size='large'
                      className='w-[150px]'
                      onClick={() => removeAward(user.id)}
                    >
                      Remove Award
                    </Button>
                  ) : (
                    <Button type='primary' size='large' className='w-[150px]' onClick={() => giveAward(user.id)}>
                      Give Award
                    </Button>
                  )}
                </Flex>
              ))}
            </Space>
          </Card>
        </div>
      </Spin>
      <CreateUpdatePost isOpen={open} setModal={setOpen} />
    </BaseLayout>
  )
}
