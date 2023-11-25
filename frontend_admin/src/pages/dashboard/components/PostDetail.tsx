import { Avatar, Card, Checkbox, Flex, Space } from 'antd'
import { CheckboxChangeEvent } from 'antd/es/checkbox'
import { ArrowDownOutlined, ArrowUpOutlined } from '@ant-design/icons'
import { CardProps } from 'antd/lib'
import dayjs from 'dayjs'
import relativeTime from 'dayjs/plugin/relativeTime'
import { ReactNode, useEffect, useState } from 'react'
dayjs.extend(relativeTime)

import { Swiper, SwiperSlide } from 'swiper/react'

import 'swiper/css'
import 'swiper/css/navigation'
import 'swiper/css/pagination'

import { Keyboard, Mousewheel, Navigation, Pagination } from 'swiper/modules'
export interface PostDetailProps extends CardProps {
  title?: string
  description?: string
  avatar?: string
  author?: string
  time?: Date
  checked?: boolean
  slideContent?: ReactNode[]
  reporterName?: string
  reportDate?: Date
  upvote?: number
  downvote?: number
  reports?: number
  major?: string[]
  subject?: string[]
  reporterAvt?: string
  reportContent?: string
  handleChangeStatus?: (value: boolean) => void
  onApprove?: () => void
  onDeny?: () => void
}

const PostDetail = ({
  title = '',
  description = '',
  avatar = 'https://xsgames.co/randomusers/avatar.php?g=pixel&key=1',
  author,
  time,
  handleChangeStatus,
  checked,
  slideContent,
  reportDate,
  reporterName,
  reporterAvt,
  reportContent,
  ...props
}: PostDetailProps) => {
  const [checkedBox, setCheckedBox] = useState(checked ?? false)

  useEffect(() => {
    setCheckedBox(checked ?? false)
  }, [checked])

  const onChange = (e: CheckboxChangeEvent) => {
    setCheckedBox(e.target.checked)
    handleChangeStatus?.(e.target.checked)
  }

  return (
    <Card className='mb-8' {...props}>
      <Flex justify='space-between' className='mb-5'>
        <div className='flex'>
          <Checkbox className='hidden' onChange={onChange} checked={checkedBox}></Checkbox>
          <div className='ml-2 flex gap-2'>
            <div>
              <Avatar className='w-9 h-9 rounded-[50%]' src={reporterAvt} alt={title} />
            </div>
            <div>
              <h1 className='text-base font-semibold'>{reporterName}</h1>
              <p>{dayjs(reportDate).fromNow()}</p>
            </div>
          </div>
        </div>
        
      </Flex>
      <p className='my-3'>{reportContent}</p>
      <Card {...props}>
        <div className='ml-2 flex gap-2'>
          <div>
            <Avatar className='w-9 h-9 rounded-[50%]' src={avatar} alt={title} />
          </div>
          <div>
            <h1 className='text-base font-semibold'>{author}</h1>
            <p>
              <b>Majors:</b> {props.major?.join(', ')}  
            </p>
            <p>
              <b>Subjects:</b> {props.subject?.join(', ')}
            </p>
            <p>
              <b>Reports:</b> {props?.reports}
            </p>
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
        <Space size={20} className='mt-5'>
          <div>
            <ArrowUpOutlined /> {props?.upvote}
          </div>
          <div>
            <ArrowDownOutlined /> {props?.downvote}
          </div>
        </Space>
      </Card>
    </Card>
  )
}

export default PostDetail
