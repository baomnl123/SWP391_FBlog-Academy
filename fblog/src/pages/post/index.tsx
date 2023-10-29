import { Button, message } from 'antd'
import { useCallback, useState } from 'react'
import PostDetail from './components/PostDetail'

const fakeData = [
  {
    id: '1',
    title:
      'Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae deleniti quos ipsam dicta, dolorum hic magni Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae deleniti quos ipsam dicta, dolorum hic magni Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae deleniti quos ipsam dicta, dolorum hic magni',
    description:
      "<img width='750' src='https://platinumlist.net/guide/wp-content/uploads/2023/03/IMG-worlds-of-adventure.webp' />",
    author: 'Dong Dang Duong',
    time: 1698300520000
  },
  {
    id: '2',
    title: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae deleniti quos ipsam dicta, dolorum hic magni',
    description:
      "<iframe width='700' height='315' src='https://www.youtube.com/embed/6lHvks6R6cI?si=yXmJ3TooM6X2rWbo' title='YouTube video player' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share' ></iframe>",
    avatar: 'https://xsgames.co/randomusers/avatar.php?g=pixel&key=1',
    author: 'Dong Dang Duong',
    time: 1698300520000
  },
  {
    id: '3',
    title: 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae deleniti quos ipsam dicta, dolorum hic magni',
    description:
      "<iframe width='700' height='315' src='https://www.youtube.com/embed/6lHvks6R6cI?si=yXmJ3TooM6X2rWbo' title='YouTube video player' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share' ></iframe>",
    avatar: 'https://xsgames.co/randomusers/avatar.php?g=pixel&key=1',
    author: 'Dong Dang Duong',
    time: 1698300520000
  }
]

export default function Post() {
  const [listDataSelected, setDataSelected] = useState<string[]>([])

  const handleAddItem = useCallback(
    (id: string, value: boolean) => {
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

  const onApprove = useCallback(() => {
    message.success({
      content: 'Approve succeeded'
    })
    setDataSelected([])
  }, [])
  const onCancel = useCallback(() => {
    message.success({
      content: 'Cancel succeeded'
    })
    setDataSelected([])
  }, [])

  return (
    <div>
      <div
        className={`text-right mb-4 duration-300 fixed top-0 right-0 z-[999] h-16 shadow-sm leading-8 bg-white dark:bg-black flex items-center justify-end w-[calc(100%_-_200px)] ${
          !listDataSelected.length ? 'opacity-0 invisible' : 'opacity-100 visible'
        }`}
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
      </div>
      <div>
        {fakeData.map((item) => {
          const { time, title, description, avatar, author, id } = item
          return (
            <PostDetail
              key={id}
              time={time}
              title={title}
              description={description}
              avatar={avatar}
              author={author}
              handleChangeStatus={(value) => handleAddItem(id, value)}
              checked={listDataSelected.includes(id)}
              className='max-w-[750px] mx-auto mb-8'
            />
          )
        })}
      </div>
    </div>
  )
}
