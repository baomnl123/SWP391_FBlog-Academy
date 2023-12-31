import { Avatar, Button, Card, Checkbox, Flex, Space } from 'antd'
import { CheckboxChangeEvent } from 'antd/es/checkbox'
import { CardProps } from 'antd/lib'
import dayjs from 'dayjs'
import { ReactNode, useEffect, useState } from 'react'
import { ArrowDownOutlined, ArrowUpOutlined } from '@ant-design/icons'
import relativeTime from 'dayjs/plugin/relativeTime'
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
  reporterAvt?: string
  reportContent?: string
  handleChangeStatus?: (value: boolean) => void
  onApprove?: () => void
  onDeny?: () => void
  upvote?: number
  downvote?: number
  reports?: number
  major?: string[]
  subject?: string[]
  
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
  onApprove,
  onDeny,
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
              <p>{dayjs(reportDate).format('YYYY-MM-DD')}</p>
            </div>
          </div>
        </div>
        <Space direction='vertical' size={10}>
          <Button block onClick={() => onApprove?.()}>
            Approve
          </Button>
          <Button block danger onClick={() => onDeny?.()}>
            Deny
          </Button>
        </Space>
      </Flex>
      <p className='my-3'>{reportContent}</p>
      <Card {...props}>
        <div className='ml-2 flex gap-2'>
          <div>
            <Avatar className='w-9 h-9 rounded-[50%]' src={avatar} alt={title} />
          </div>
          <div>
            <h1 className='text-base font-semibold'>{author}</h1>
            <p>{dayjs(time).format('YYYY-MM-DD')}</p>
            <p>
              <b>Majors:</b> {props.major?.join(', ')}  
            </p>
            <p>
              <b>Subjects:</b> {props.subject?.join(', ')}
            </p>
            <p>
              <b>Reports:</b> {props?.reports}
            </p>
            
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
