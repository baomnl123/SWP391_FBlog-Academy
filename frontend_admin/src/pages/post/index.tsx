import api from '@/config/api'
import { useRequest } from 'ahooks'
import { Image, Spin, message } from 'antd'
import { useCallback, useState } from 'react'
import PostDetail from './components/PostDetail'

export default function Post() {
  const [listDataSelected, setDataSelected] = useState<number[]>([])

  const {
    data: posts,
    loading: postLoading,
    refresh
  } = useRequest(async () => {
    try {
      const res = api.reportPostPending()
      if (res) {
        return res
      }
    } catch (error) {
      console.log(error)
    }
  })

  const { runAsync: approvePost } = useRequest(api.approvePost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Approve post success')
        refresh()
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const { runAsync: denyPost } = useRequest(api.denyPost, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Deny post success')
        refresh()
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })

  const handleAddItem = useCallback(
    (id: number, value: boolean) => {
      const newList = [...listDataSelected]
      const index = newList.findIndex((item) => item === id)
      if (index === -1 && value) {
        newList.push(id)
      } else if (index !== -1 && !value) {
        newList.splice(index, 1)
      }
      setDataSelected(newList)
    },
    [listDataSelected]
  )

  // const onApprove = useCallback(() => {
  //   console.log('listDataSelected', listDataSelected)
  //   // approvePost()
  //   const id = getLocalStorage('id')
  //   approvePost(Number(id ?? 0), listDataSelected[0])
  //   setDataSelected([])
  // }, [approvePost, listDataSelected])

  // const onCancel = useCallback(() => {
  //   const id = getLocalStorage('id')
  //   denyPost(Number(id ?? 0), listDataSelected[0])
  //   setDataSelected([])
  // }, [denyPost, listDataSelected])

  return (
    <Spin spinning={postLoading}>
      {/* <div
        className={`text-right mb-4 duration-300 fixed top-0 right-0 z-[999] h-16 shadow-sm leading-8 bg-white dark:bg-black flex items-center justify-end w-[calc(100%_-_200px)] opacity-0 invisible`}
      >
        <Button type='primary' onClick={onApprove}>
          Approve
        </Button>
        <Button type='primary' danger className='ml-2' onClick={onCancel}>
          Deny
        </Button>
        <Button type='primary' className='ml-2 mr-4' style={{ background: '#ccc' }} onClick={() => setDataSelected([])}>
          Cancel
        </Button>
      </div> */}
      <div>
        {posts?.map((item) => {
          const { id, createdAt, title, content, user, images, videos } = item.post
          return (
            <div key={id}>
              <PostDetail
                reporterAvt={item?.reporter?.avatarUrl}
                reporterName={item?.reporter?.name}
                reportDate={item?.reporter?.createdAt}
                onApprove={async () => {
                  await approvePost(item?.reporter?.id, id)
                }}
                onDeny={async () => {
                  await denyPost(item?.reporter?.id, id)
                }}
                time={createdAt}
                title={title}
                description={content}
                avatar={user?.avatarUrl ?? ''}
                author={user?.name}
                handleChangeStatus={(value) => handleAddItem(id, value)}
                checked={listDataSelected.includes(id)}
                className='max-w-[750px] mx-auto mb-8'
                slideContent={[
                  ...(images ?? []).map((image) => (
                    <Image
                      key={image.id}
                      src={image.url}
                      className='w-[1194px] h-[620px]'
                      placeholder='https://i0.wp.com/thinkfirstcommunication.com/wp-content/uploads/2022/05/placeholder-1-1.png?w=1200&ssl=1'
                    />
                  )),
                  ...(videos ?? []).map((video) => (
                    <video controls className='w-full'>
                      <source src={video.url} type='video/mp4' className='object-contain' />
                    </video>
                  ))
                ]}
              />
            </div>
          )
        })}
      </div>
    </Spin>
  )
}
