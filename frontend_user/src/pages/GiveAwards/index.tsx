import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import { PlusOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Avatar, Button, Card, Flex, Space, Spin, Typography, message } from 'antd'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import CreateUpdatePost from '../Dashboard/components/CreateUpdatePost'
import SubSide from '../Dashboard/components/SubSide'
import { PendingPost } from '@/types'
import { FilterType } from '../Dashboard/components/SubSide'
export default function GiveAwards() {
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()
  const [open, setOpen] = useState(false)
  const [idPost, setIdPost] = useState<number | undefined>()
  const [openPost, setOpenPost] = useState(false)
  const [categories, setCategories] = useState<string | string[] | number | number[] | null>([])
  const [tag, setTags] = useState<string | string[] | number | number[]>([])
  const [filter, setFilter] = useState<FilterType | null>(null)
  const [postFilter, setPostFilter] = useState<PendingPost[] | null>(null)

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
          <SubSide />
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
