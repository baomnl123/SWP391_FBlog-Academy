import api from '@/api'
import { RootState } from '@/store'

import { useRequest } from 'ahooks'
import { Card } from 'antd'
import { useState } from 'react'
import { useSelector } from 'react-redux'

interface RightSiderDashboardMenuProps {
  title?: string
  data?: { title: string; value: number }[]
  onChange?: (value: number) => void
  idSelect?: number
}
const RightSiderDashboardMenu = ({ title, data, onChange, idSelect }: RightSiderDashboardMenuProps) => {
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)

  return (
    <Card size='small' title={title ?? ''} className='w-full text-center mb-8'>
      {data?.map((item) => {
        return (
          <div
            className={`cursor-pointer py-1 ${!isDarkMode && idSelect === item.value ? 'bg-[#C7DCF0]' : ''} ${
              isDarkMode && idSelect === item.value ? 'bg-[#0F2438]' : ''
            }`}
            key={item.value}
            onClick={() => onChange?.(item.value)}
          >
            {item.title}
          </div>
        )
      })}
    </Card>
  )
}

const RightSiderDashboard = ({
  onMajorChange,
  onSubjectChange,
  onPostChange
}: {
  onMajorChange?: (value?: number) => void
  onSubjectChange?: (value?: number) => void
  onPostChange?: (value?: string) => void
}) => {
  const [majorSelect, setMajorSelect] = useState(-1)
  const [subjectSelect, setSubjectSelect] = useState(-1)
  const [postSelect, setPostSelect] = useState(-1)
  const { data: trendingMajor } = useRequest(api.trendingMajor, {
    onError(e) {
      console.error(e)
    }
  })

  const { data: trendingSubject } = useRequest(api.trendingSubject, {
    onError(e) {
      console.error(e)
    }
  })

  const { data: trendingPost } = useRequest(api.trendingPost, {
    onError(e) {
      console.error(e)
    }
  })

  return (
    <div className='px-8 mt-[80px] max-w-[300px] w-full fixed right-0'>
      {(trendingMajor ?? []).length > 0 && (
        <RightSiderDashboardMenu
          onChange={(value) => {
            setMajorSelect(majorSelect === value ? -1 : value)
            onMajorChange?.(majorSelect === value ? undefined : value)
            // Reset the selection of Trending Post
            setPostSelect(-1)
          }}
          idSelect={majorSelect}
          title='Trending Major'
          data={(trendingMajor ?? []).map((item) => ({
            title: item.majorName,
            value: item.id
          }))}
        />
      )}

      {(trendingSubject ?? []).length > 0 && (
        <RightSiderDashboardMenu
          onChange={(value) => {
            setSubjectSelect(subjectSelect === value ? -1 : value)
            onSubjectChange?.(subjectSelect === value ? undefined : value)
            // Reset the selection of Trending Post
            setPostSelect(-1)
          }}
          title='Trending Subject'
          idSelect={subjectSelect}
          data={(trendingSubject ?? []).map((item) => ({
            title: item.subjectName,
            value: item.id
          }))}
        />
      )}

      {(trendingPost ?? []).length > 0 && (
        <RightSiderDashboardMenu
          onChange={(value) => {
            const post = trendingPost?.find((el) => el.id === value)
            setPostSelect(postSelect === value ? -1 : value)
            onPostChange?.(postSelect === value ? undefined : post?.title)
            // Reset the selection of Major and Subject
            setMajorSelect(-1)
            setSubjectSelect(-1)
          }}
          title='Trending Post'
          idSelect={postSelect}
          data={(trendingPost ?? []).map((item) => ({
            title: item.title,
            value: item.id
          }))}
        />
      )}
    </div>
  )
}

export default RightSiderDashboard
