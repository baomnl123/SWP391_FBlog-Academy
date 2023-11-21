import api from '@/api'
import { Comment } from '@/api/types/comment'
import IconRightLong from '@/assets/images/svg/IconRightLong'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Avatar, Form, Input, Modal, Typography } from 'antd'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'

interface ModalCommentProps {
  isOpen: boolean
  setModal?: (value: boolean) => void
  idPost?: number
  onClose: () => void
}

const ModalComment = ({ isOpen, setModal, idPost }: ModalCommentProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)
  const [comments, setComments] = useState<Comment[]>([])

  useEffect(() => {
    setIsModalOpen(isOpen)
  }, [isOpen])

  const handleOk = () => {
    form.submit()
  }

  const { runAsync: getComments } = useRequest(api.getCommentByPost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        setComments(res as unknown as Comment[])
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const { runAsync: sendComments } = useRequest(api.createCommentByPost, {
    manual: true,
    onSuccess: (res) => {
      console.log(res)
      if (res) {
        getComments({
          postId: idPost ?? 0
        })
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  useEffect(() => {
    if (!idPost) return
    getComments({
      postId: idPost
    })
  }, [getComments, idPost])

  const handleCancel = () => {
    form.resetFields()
    setIsModalOpen(false)
    setModal?.(false)
  }

  const onFinish = (value: { comment: string }) => {
    sendComments({
      content: value.comment,
      postId: idPost ?? 0,
      userId: user?.id ?? 0
    })
    // setIsModalOpen(false)
    // setModal?.(false)
    form.resetFields()
  }

  return (
    <Modal open={isModalOpen} onOk={handleOk} onCancel={handleCancel} footer={null}>
      <Typography.Title level={3} className='text-center mt-3'>
        Comment
      </Typography.Title>
      <div className='max-h-[400px] overflow-y-scroll mb-10 custom-bar'>
        {comments.map((comment: Comment) => {
          return (
            <div className='flex mb-10' key={comment.id}>
              <Avatar src={comment?.user?.avatarUrl} alt='' className='w-12 h-12 rounded-[50%]' />
              <div className='ml-3 w-[calc(100%_-_60px)]'>
                <Typography.Title level={4}>{comment?.user?.name}</Typography.Title>
                <Typography>{comment.content}</Typography>
              </div>
            </div>
          )
        })}
      </div>

      <Form form={form} onFinish={onFinish}>
        <Form.Item name='comment' rules={[{ required: true, message: 'Please input your comment!' }]}>
          <Input
            suffix={
              <div onClick={handleOk}>
                <IconRightLong color='#4e3b3b' />
              </div>
            }
            placeholder='Comment'
            className='h-[70px]'
          />
        </Form.Item>
      </Form>
    </Modal>
  )
}

export default ModalComment
