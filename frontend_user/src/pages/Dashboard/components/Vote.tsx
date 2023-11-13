import api from '@/api'
import IconDownLong from '@/assets/images/svg/IconDownLong'
import IconUpLong from '@/assets/images/svg/IconUpLong'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Button, Typography } from 'antd'
import { useCallback, useState } from 'react'
import { useSelector } from 'react-redux'
import ModalUserVote from './ModalUserVote'
import { User } from '@/types'

const Vote = ({
  vote,
  userId,
  postId,
  upvote,
  downvote,
  usersVote,
  onVoteSuccess
}: {
  vote: number
  userId?: number
  postId?: number
  upvote?: boolean
  downvote?: boolean
  usersVote?: User[]
  onVoteSuccess?: () => void
}) => {
  const [open, setOpen] = useState(false)
  const { runAsync: votePost } = useRequest(api.votePost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        onVoteSuccess?.()
      }
      console.log(res)
    }
  })

  const { runAsync: voteUpdate } = useRequest(api.voteUpdate, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        onVoteSuccess?.()
      }
    }
  })

  const onRemoveVote = useCallback(async () => {
    try {
      if (postId && userId) {
        await api.deleteVote(postId, userId)
        onVoteSuccess?.()
      }
    } catch (e) {
      console.error(e)
    }
  }, [postId, userId, onVoteSuccess])

  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  return (
    <>
      <Button
        onClick={async () => {
          if (upvote) {
            await onRemoveVote()
          } else {
            if (!upvote && !downvote) {
              await votePost({
                currentUserId: userId ?? 0,
                postId: postId ?? 0,
                vote: true
              })
            } else {
              await voteUpdate({
                currentUserId: userId ?? 0,
                postId: postId ?? 0,
                vote: true
              })
            }
          }
        }}
      >
        <IconUpLong width={15} height={15} color={upvote && !downvote ? 'blue' : isDarkMode ? '#fff' : '#000'} />
      </Button>
      <Typography className='min-w-[50px] cursor-pointer' onClick={() => setOpen(true)}>
        {vote}
      </Typography>
      <Button
        onClick={async () => {
          if (downvote) {
            await onRemoveVote()
          } else {
            if (!upvote && !downvote) {
              await votePost({
                currentUserId: userId ?? 0,
                postId: postId ?? 0,
                vote: false
              })
            } else {
              await voteUpdate({
                currentUserId: userId ?? 0,
                postId: postId ?? 0,
                vote: false
              })
            }
          }
        }}
      >
        <IconDownLong width={15} height={15} color={downvote && !upvote ? 'blue' : isDarkMode ? '#fff' : '#000'} />
      </Button>
      <ModalUserVote open={open} users={usersVote ?? ([] as User[])} footer={false} onCancel={() => setOpen(false)} />
    </>
  )
}

export default Vote
