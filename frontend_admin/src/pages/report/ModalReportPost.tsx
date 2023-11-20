import { Avatar, Card, Modal, ModalProps, Pagination } from 'antd'
import { Swiper, SwiperSlide } from 'swiper/react'

import 'swiper/css'
import 'swiper/css/navigation'
import 'swiper/css/pagination'
import dayjs from 'dayjs'
import { Keyboard, Mousewheel, Navigation } from 'swiper/modules'

type PostInfo = {
  avatar: string
  title: string
  description: string
  slideContent: string[]
  time: string
  author: string
}

export default function ModalReportPost(props: ModalProps & PostInfo) {
  const { avatar, title, description, slideContent, time, author, ...rest } = props
  return (
    <Modal {...rest}>
      <Card>
        <div className='ml-2 flex gap-2'>
          <div>
            <Avatar className='w-9 h-9 rounded-[50%]' src={avatar} alt={title} />
          </div>
          <div>
            <h1 className='text-base font-semibold'>{author}</h1>
            <p>{dayjs(time).fromNow()}</p>
          </div>
        </div>
        <div className='mt-4'>
          <p className='text-base'>
            <div className='mt-3'>{description && <div dangerouslySetInnerHTML={{ __html: title }} />}</div>
          </p>
          <div className='my-3'>{description && <div dangerouslySetInnerHTML={{ __html: description }} />}</div>
          <Swiper
            cssMode={true}
            navigation={true}
            pagination={true}
            mousewheel={true}
            keyboard={true}
            modules={[Navigation, Pagination, Mousewheel, Keyboard]}
            className='mySwiper rounded-3xl py-'
          >
            {slideContent?.map((slide, index) => <SwiperSlide key={index}>{slide}</SwiperSlide>)}
          </Swiper>
        </div>
      </Card>
    </Modal>
  )
}
