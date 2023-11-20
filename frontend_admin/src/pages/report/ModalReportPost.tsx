import api from '@/config/api'
import { Post } from '@/types'
import { ArrowDownOutlined, ArrowUpOutlined } from '@ant-design/icons'
import { Avatar, Button, Card, Flex, Image, Modal, ModalProps, Space, message } from 'antd'
import dayjs from 'dayjs'
import { useCallback } from 'react'
import { Keyboard, Mousewheel, Navigation, Pagination } from 'swiper/modules'
import { Swiper, SwiperSlide } from 'swiper/react'

export default function ModalReportPost(props: ModalProps & { data: Post; onSuccess?: () => void }) {
  const { data, onSuccess, ...rest } = props

  const onApprove = useCallback(async () => {
    try {
      await api.approvePost(data?.reportList[0]?.reporter.id, data?.id)
      message.success('approve success')
      onSuccess?.()
    } catch (e) {
      console.error(e)
    }
  }, [data?.id, data?.reportList, onSuccess])

  const onDeny = useCallback(async () => {
    try {
      await api.denyPost(data?.reportList[0]?.reporter.id, data?.id)
      message.success('deny success')
      onSuccess?.()
    } catch (e) {
      console.error(e)
    }
  }, [data?.id, data?.reportList, onSuccess])

  return (
    <Modal {...rest} width={rest.width ? rest.width : 700}>
      <Card>
        <Flex justify='space-between' align='center'>
          <Space size={20}>
            <Avatar src={data?.reportList[0]?.reporter.avatarUrl} />
            <div>
              <p>{data?.reportList[0]?.reporter.name}</p>
              <p>{dayjs(data?.reportList[0]?.createdAt).format('YYYY-MM-DD')}</p>
            </div>
          </Space>
          <Space direction='vertical'>
            <Button block onClick={onApprove}>
              Approve
            </Button>
            <Button danger block onClick={onDeny}>
              Deny
            </Button>
          </Space>
        </Flex>
        <p className='mb-10'>{data?.reportList[0]?.content}</p>
        <Card>
          <div className='ml-2 flex gap-2'>
            <div>
              <Avatar className='w-9 h-9 rounded-[50%]' src={data?.user.avatarUrl} alt={data?.user.name} />
            </div>
            <div>
              <h1 className='text-base font-semibold'>{data?.user.name}</h1>
              <p>{dayjs(data?.user.createdAt).fromNow()}</p>
            </div>
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
      </Card>
    </Modal>
  )
}
