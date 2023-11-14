import { User } from '@/types'
import { Avatar, Empty, Modal, ModalProps, Space, Typography } from 'antd'
import { useNavigate } from 'react-router-dom'

export default function ModalUserVote(props: ModalProps & { users: User[] }) {
  const { users, ...rest } = props
  const navigate = useNavigate()
  return (
    <Modal {...rest} title='User vote'>
      {users.length > 0 ? (
        users.map((user) => (
          <Space className='w-full my-5 cursor-pointer' onClick={() => navigate(`/profile/${user?.id}`)} size={10}>
            <Avatar src={user?.avatarUrl} />
            <Typography.Text>{user?.name}</Typography.Text>
          </Space>
        ))
      ) : (
        <Empty />
      )}
    </Modal>
  )
}
