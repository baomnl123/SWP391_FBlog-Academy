import { Avatar, Button, Card, Image, Modal, ModalProps, Space, message } from 'antd'
import { Swiper, SwiperSlide } from 'swiper/react'

import dayjs from 'dayjs'
import 'swiper/css'
import 'swiper/css/navigation'
import 'swiper/css/pagination'

import { Post } from '@/types'
import { ArrowDownOutlined, ArrowUpOutlined } from '@ant-design/icons'
import { Keyboard, Mousewheel, Navigation, Pagination } from 'swiper/modules'
import { useCallback } from 'react'
import api from '@/config/api'

export default function ModalPost(props: ModalProps & { data: Post; onBan?: () => void }) {
  const { data, ...rest } = props
  const onDelete = useCallback(
    async (id: number) => {
      try {
        await api.deletPost(id)
        rest.onBan?.()
      } catch (e) {
        console.error(e)
      }
    },
    [rest]
  )

  return (
    <Modal {...rest} width={rest.width ? rest.width : 700}>
      <Card>
        <div className='flex justify-between items-center'>
          <div className='ml-2 flex gap-2'>
            <div>
              <Avatar className='w-9 h-9 rounded-[50%]' src={data?.user.avatarUrl} alt={data?.user.name} />
            </div>
            <div>
              <h1 className='text-base font-semibold'>{data?.user.name}</h1>
              <p>{dayjs(data?.user.createdAt).fromNow()}</p>
            </div>
          </div>
          <Button
            danger
            onClick={() => {
              onDelete(data?.id)
              message.success('Delete post success')
            }}
          >
            Delete
          </Button>
        </div>
        <div className='mt-4'>
          <div className='text-base'>
            <p className='mt-3'>{data?.title}</p>
          </div>
          <div className='my-3'>{data?.content}</div>
          <Swiper
            cssMode={true}
            navigation={true}
            pagination={true}
            mousewheel={true}
            keyboard={true}
            modules={[Navigation, Pagination, Mousewheel, Keyboard]}
            className='mySwiper rounded-3xl py-'
          >
            {[...(data?.images ?? []).map((image) => image.url)]?.map((slide, index) => (
              <SwiperSlide key={index}>
                <Image src={slide} alt={slide} />
              </SwiperSlide>
            ))}
            {[...(data?.videos ?? []).map((video) => video.url)]?.map((slide, index) => (
              <SwiperSlide key={index}>
                <video controls className='w-full'>
                  <source src={slide} type='video/mp4' className='object-contain' />
                </video>
              </SwiperSlide>
            ))}
          </Swiper>
        </div>
        <Space size={20} className='mt-5'>
          <div>
            <ArrowUpOutlined /> {data?.upvotes}
          </div>
          <div>
            <ArrowDownOutlined /> {data?.downvotes}
          </div>
        </Space>
      </Card>
    </Modal>
  )
}
