import { Card as AntCard, Avatar, Flex, Space, Typography } from 'antd'
import dayjs from 'dayjs'
import { ReactNode } from 'react'
import { Swiper, SwiperSlide } from 'swiper/react'

import 'swiper/css'
import 'swiper/css/navigation'
import 'swiper/css/pagination'

import { Keyboard, Mousewheel, Navigation, Pagination } from 'swiper/modules'

export default function Card({
  footer,
  slideContent,
  action,
  user,
  createDate,
  title,
  content,
  onClickAvatar,
  className,
  showFollow = false,
  onFollow
}: {
  footer?: ReactNode[]
  slideContent?: ReactNode[]
  action: ReactNode[]
  user: {
    username: string
    avatar?: string
    isFollow?: boolean
  }
  createDate?: string
  title: string
  content: string
  onClickAvatar?: () => void
  className?: string
  showFollow?: boolean
  onFollow?: () => void
}) {
  return (
    <AntCard className={`max-w-[1200px] p-5 ${className}`} actions={footer}>
      <Flex justify='space-between' align='center'>
        <Space size={10} onClick={() => onClickAvatar?.()} className='cursor-pointer'>
          <Avatar size={64} src={user?.avatar} />
          <Space direction='vertical'>
            <Space size={10} align='center'>
              <Typography.Text className='font-semibold text-lg'>{user?.username}</Typography.Text>
              {showFollow && (
                <Typography.Link
                  onClick={(e) => {
                    e.stopPropagation()
                    onFollow?.()
                  }}
                >
                  {user.isFollow ? 'Unfollow' : 'Follow'}
                </Typography.Link>
              )}
            </Space>
            <Typography.Text>{createDate ?? dayjs().format('DD/MM/YYYY')}</Typography.Text>
          </Space>
        </Space>
        <Space direction='vertical' size={5}>
          {action}
        </Space>
      </Flex>
      <Space className='w-full my-5' direction='vertical' size={10}>
        <div>
          <Typography.Title level={2}>{title}</Typography.Title>
          <Typography.Paragraph className='text-base'>{content}</Typography.Paragraph>
        </div>
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
      </Space>
    </AntCard>
  )
}
