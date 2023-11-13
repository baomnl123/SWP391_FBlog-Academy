import api from '@/api'
import { RootState } from '@/store'
import { User } from '@/types'
import { getLocalStorage } from '@/utils/helpers'
import { CheckCircleFilled } from '@ant-design/icons'
import { Avatar, Button, Empty, Flex, Modal, ModalProps, Space, Typography } from 'antd'
import { useCallback } from 'react'
import { useSelector } from 'react-redux'

export default function ModalListUsers(props: ModalProps & { users: User[]; onToggleFollow?: () => void }) {
  const { users, onToggleFollow, ...rest } = props
  const id = useSelector<RootState>((state) => state.userReducer.user?.id)
  const currentId = Number(id ?? getLocalStorage('id'))

  const follow = useCallback(
    async (userId: number) => {
      try {
        await api.follow(currentId, userId)
        onToggleFollow?.()
      } catch (e) {
        console.error(e)
      }
    },
    [currentId, onToggleFollow]
  )

  const unFollow = useCallback(
    async (userId: number) => {
      try {
        await api.unFollow(currentId, userId)
        onToggleFollow?.()
      } catch (e) {
        console.error(e)
      }
    },
    [currentId, onToggleFollow]
  )

  return (
    <Modal {...rest}>
      <Space className='w-full' direction='vertical' size={20}>
        {(users ?? []).length > 0 ? (
          users?.map((user) => (
            <Flex justify='space-between' align='center' key={user.id}>
              <Space size={10}>
                <Avatar src={user.avatarUrl} alt={user.name} size='large' />
                <Space direction='vertical' size={5}>
                  <Space size={5}>
                    <Typography.Text className='font-semibold'>{user.name}</Typography.Text>
                    {user.isAwarded && <CheckCircleFilled className='text-blue-600' />}
                  </Space>
                  <Typography.Text>{user.email}</Typography.Text>
                </Space>
              </Space>
              {currentId !== user.id && (
                <Button
                  className='w-[100px]'
                  onClick={() => {
                    user.isFollowed ? unFollow(user.id) : follow(user.id)
                  }}
                >
                  {user.isFollowed ? 'Unfollow' : 'Follow'}
                </Button>
              )}
            </Flex>
          ))
        ) : (
          <Empty />
        )}
      </Space>
    </Modal>
  )
}
