import api from '@/api'
import { RootState } from '@/store'
import { Post } from '@/types'
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
  onCategoryChange,
  onTagChange,
  onPostChange
}: {
  onCategoryChange?: (value?: number) => void
  onTagChange?: (value?: number) => void
  onPostChange?: (value?: Post) => void
}) => {
  const [categorySelect, setCategorySelect] = useState(-1)
  const [tagSelect, setTagSelect] = useState(-1)
  const [postSelect, setPostSelect] = useState(-1)
  const { data: trendingCategory } = useRequest(api.trendingCategory, {
    onError(e) {
      console.error(e)
    }
  })

  const { data: trendingTag } = useRequest(api.trendingTag, {
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
      {(trendingCategory ?? []).length > 0 && (
        <RightSiderDashboardMenu
          onChange={(value) => {
            setCategorySelect(categorySelect === value ? -1 : value)
            onCategoryChange?.(categorySelect === value ? undefined : value)
          }}
          idSelect={categorySelect}
          title='Trending Category'
          data={(trendingCategory ?? []).map((item) => ({
            title: item.majorName,
            value: item.id
          }))}
        />
      )}
      {(trendingTag ?? []).length > 0 && (
        <RightSiderDashboardMenu
          onChange={(value) => {
            setTagSelect(tagSelect === value ? -1 : value)
            onTagChange?.(tagSelect === value ? undefined : value)
          }}
          title='Trending Tag'
          idSelect={tagSelect}
          data={(trendingTag ?? []).map((item) => ({
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
            onPostChange?.(postSelect === value ? undefined : post)
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
