import api from '@/api'
import IconDownLong from '@/assets/images/svg/IconDownLong'
import IconUpLong from '@/assets/images/svg/IconUpLong'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Button, Typography } from 'antd'
import { useState } from 'react'
import { useSelector } from 'react-redux'

const Vote = ({
  vote,
  userId,
  postId,
  upvote,
  downvote,
  onVoteSuccess
}: {
  vote: number
  userId?: number
  postId?: number
  upvote?: boolean
  downvote?: boolean
  onVoteSuccess?: () => void
}) => {
  const { runAsync: votePost } = useRequest(api.votePost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        // setCount(count + 1)
        onVoteSuccess?.()
      }
      console.log(res)
    }
  })
  const { runAsync: votePost2 } = useRequest(api.votePost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        // setCount(count - 1)
        onVoteSuccess?.()
      }
      console.log(res)
    }
  })

  const { runAsync: voteUpdate } = useRequest(api.voteUpdate, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        // setCount(count + 1)
        onVoteSuccess?.()
      }
      console.log(res)
    }
  })
  const { runAsync: voteUpdate2 } = useRequest(api.voteUpdate, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        // setCount(count - 1)
        onVoteSuccess?.()
      }
      console.log(res)
    }
  })
  const [count] = useState(vote)
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  return (
    <>
      <Button
        onClick={async () => {
          if (upvote && !downvote) return
          if (!upvote && !downvote) {
            await votePost({
              currentUserId: userId ?? 0,
              postId: postId ?? 0,
              vote: 'true'
            })
          }
          await voteUpdate({
            currentUserId: userId ?? 0,
            postId: postId ?? 0,
            vote: 'true'
          })
        }}
      >
        <IconUpLong width={15} height={15} color={upvote && !downvote ? 'blue' : isDarkMode ? '#fff' : '#000'} />
      </Button>
      <Typography className='min-w-[50px]'>{count}</Typography>
      <Button
        onClick={async () => {
          if (downvote && !upvote) return
          if (!upvote && !downvote) {
            await votePost2({
              currentUserId: userId ?? 0,
              postId: postId ?? 0,
              vote: 'false'
            })
          }
          await voteUpdate2({
            currentUserId: userId ?? 0,
            postId: postId ?? 0,
            vote: 'false'
          })
        }}
      >
        <IconDownLong width={15} height={15} color={downvote && !upvote ? 'blue' : isDarkMode ? '#fff' : '#000'} />
      </Button>
    </>
  )
}

export default Vote
