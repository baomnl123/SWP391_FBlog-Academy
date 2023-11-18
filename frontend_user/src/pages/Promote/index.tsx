import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import { useRequest } from 'ahooks'
import { Avatar, Button, Card, Flex, Space, Spin, Typography, message } from 'antd'
import { useCallback, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import SubSide from '../Dashboard/components/SubSide'

export default function Promote() {
  const navigate = useNavigate()
  const [loading, setLoading] = useState(false)
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

  const promote = useCallback(
    async (id: number) => {
      setLoading(true)
      try {
        await api.promote(id)
        refresh()
        message.success('Promote user successfully')
      } catch (e) {
        console.error(e)
      } finally {
        setLoading(false)
      }
    },
    [refresh]
  )

  const demote = useCallback(
    async (id: number) => {
      setLoading(true)
      try {
        await api.demote(id)
        refresh()
        message.success('Demote user successfully')
      } catch (e) {
        console.error(e)
      } finally {
        setLoading(false)
      }
    },
    [refresh]
  )

  return (
    <BaseLayout sider={<SubSide />}>
      <Spin spinning={loading}>
        <div className='w-full'>
          <Card className='max-w-[800px] mx-auto'>
            <Space className='w-full' size={20} direction='vertical'>
              {data?.map((user) => {
                return (
                  <Flex
                    justify='space-between'
                    align='center'
                    className='cursor-pointer'
                    onClick={() => navigate(`/profile/${user?.id}`)}
                  >
                    <Space size={20}>
                      <Avatar size={64} src={user.avatarUrl} />
                      <Typography.Text>{user.name}</Typography.Text>
                    </Space>
                    <Space size={30}>
                      <Typography.Text> {user.followerNumber} Followers</Typography.Text>
                      <Typography.Text>{user.postNumber} Post</Typography.Text>
                    </Space>
                    {user.role === 'MD' && (
                      <Button
                        type='primary'
                        danger
                        size='large'
                        className='w-[150px]'
                        onClick={(e) => {
                          e.stopPropagation()
                          demote(user.id)
                        }}
                      >
                        Demote
                      </Button>
                    )}
                    {user.role === 'SU' && (
                      <Button
                        type='primary'
                        size='large'
                        className='w-[150px]'
                        onClick={(e) => {
                          e.stopPropagation()
                          promote(user.id)
                        }}
                      >
                        Promote
                      </Button>
                    )}
                  </Flex>
                )
              })}
            </Space>
          </Card>
        </div>
      </Spin>
    </BaseLayout>
  )
}
