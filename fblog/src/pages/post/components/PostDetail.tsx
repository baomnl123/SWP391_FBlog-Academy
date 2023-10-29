import { calculateTimeAgo } from '@/utils/helpers'
import { Card, Checkbox } from 'antd'
import { CheckboxChangeEvent } from 'antd/es/checkbox'
import { CardProps } from 'antd/lib'
import { useEffect, useState } from 'react'
export interface PostDetailProps extends CardProps {
  title?: string
  description?: string
  avatar?: string
  author?: string
  time?: number
  checked?: boolean
  handleChangeStatus?: (value: boolean) => void
}

const PostDetail = ({
  title = '',
  description = '',
  avatar = 'https://xsgames.co/randomusers/avatar.php?g=pixel&key=1',
  author,
  time,
  handleChangeStatus,
  checked,
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
      <div className='flex'>
        <Checkbox onChange={onChange} checked={checkedBox}></Checkbox>
        <div className='ml-2 flex gap-2'>
          <div>
            <img className='w-9 h-9 rounded-[50%]' src={avatar} alt={title} />
          </div>
          <div>
            <h1 className='text-base font-semibold'>{author}</h1>
            <p>{calculateTimeAgo(time ?? 0)}</p>
          </div>
        </div>
      </div>

      <div className='mt-4'>
        <p className='text-base'>
          <div className='mt-3'>{description && <div dangerouslySetInnerHTML={{ __html: title }} />}</div>
        </p>
        <div className='mt-3'>{description && <div dangerouslySetInnerHTML={{ __html: description }} />}</div>
      </div>
    </Card>
  )
}

export default PostDetail
