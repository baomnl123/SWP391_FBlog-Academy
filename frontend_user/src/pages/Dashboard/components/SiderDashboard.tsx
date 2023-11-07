import api from '@/api'
import IconPhotoFilm from '@/assets/images/svg/IconPhotoFilm'
import IconPicture from '@/assets/images/svg/IconPicture'
import SelectLabel from '@/components/SelectLabel'
import { RootState } from '@/store'
import { User } from '@/types'
import { AuditOutlined, FileAddOutlined, FileDoneOutlined, FileSyncOutlined, GiftOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Button, Flex, SelectProps, Typography } from 'antd'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'

interface SiderDashboardProps {
  createPost?: () => void
  onGetTags?: (data: string[] | string | number | number[]) => void
  onGetCategories?: (data: string[] | string | number | number[]) => void
  onFilter?: (data: FilterType | null) => void
}

export type FilterType = 'image' | 'video'

const SiderDashboard = ({ createPost, onGetTags, onGetCategories, onFilter }: SiderDashboardProps) => {
  const navigate = useNavigate()
  const [filter, setFilter] = useState<FilterType | null>(null)
  const [categoryOptions, setCategoryOptions] = useState<SelectProps['options']>([])
  const [tagOptions, setTagOptions] = useState<SelectProps['options']>([])
  const userInfo = useSelector<RootState>((state) => state.userReducer.user)

  const { data: categoriesData } = useRequest(async () => {
    try {
      const res = await api.getAllCategory()
      return res
    } catch (error) {
      console.log(error)
    }
  })

  const { data: tagsData } = useRequest(async () => {
    try {
      const res = await api.getAllTag()
      return res
    } catch (error) {
      console.log(error)
    }
  })

  useEffect(() => {
    if (!categoriesData) return
    const categories: SelectProps['options'] = categoriesData.map((item) => {
      return {
        label: item.categoryName,
        value: item.id
      }
    })
    setCategoryOptions(categories)
  }, [categoriesData])

  useEffect(() => {
    if (!tagsData) return
    const options: SelectProps['options'] = tagsData.map((item) => {
      return {
        label: item.tagName,
        value: item.id
      }
    })
    setTagOptions(options)
  }, [tagsData])

  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  return (
    <div>
      <div className='mb-6'>
        <SelectLabel
          label='Tag'
          placeHolder='Select Tag'
          optionData={tagOptions}
          onChange={(value) => {
            onGetTags?.(value)
          }}
        />
      </div>
      <div className='mb-6'>
        <SelectLabel
          label='Category'
          placeHolder='Select Category'
          optionData={categoryOptions}
          onChange={(value) => {
            onGetCategories?.(value)
          }}
        />
      </div>

      <div className='mb-8'>
        {
          <Button
            icon={<FileDoneOutlined />}
            size='large'
            className={`w-full text-left mb-4 ${filter === 'image' ? 'bg-blue-600' : ''}`}
            onClick={() => {
              if (filter !== 'image') {
                setFilter('image')
                onFilter?.('image')
                return
              }
              setFilter(null)
              onFilter?.(null)
            }}
          >
            Post have Image
          </Button>
        }
        {
          <Button
            icon={<FileDoneOutlined />}
            size='large'
            className={`w-full text-left mb-4 ${filter === 'video' ? 'bg-blue-600' : ''}`}
            onClick={() => {
              if (filter !== 'video') {
                setFilter('video')
                onFilter?.('video')
                return
              }
              setFilter(null)
              onFilter?.(null)
            }}
          >
            Post have Video
          </Button>
        }
        {(userInfo as User)?.role === 'LT' && (
          <Button
            icon={<AuditOutlined />}
            size='large'
            className='w-full text-left mb-4'
            onClick={() => navigate('/promote')}
          >
            Promote
          </Button>
        )}
        {((userInfo as User)?.role === 'LT' ||
          (userInfo as User)?.role === 'MD' ||
          (userInfo as User)?.role === 'SU') && (
          <Button
            icon={<FileDoneOutlined />}
            size='large'
            className='w-full text-left mb-4'
            onClick={() => navigate('/save-list')}
          >
            Save list
          </Button>
        )}
        {((userInfo as User)?.role === 'LT' || (userInfo as User)?.role === 'MD') && (
          <>
            <Button
              className='w-full text-left mb-4'
              icon={<FileSyncOutlined />}
              size='large'
              onClick={() => navigate('/view-pending-list')}
            >
              View pending list
            </Button>
            <Button
              size='large'
              className='w-full text-left mb-4'
              icon={<GiftOutlined />}
              onClick={() => navigate('/give-awards')}
            >
              Give awards
            </Button>
          </>
        )}
        {(userInfo as User)?.role !== 'AD' ? (
          <Button size='large' icon={<FileAddOutlined />} className='w-full text-left' onClick={() => createPost?.()}>
            Create Post
          </Button>
        ) : (
          <Button size='large' className='w-full text-left'>
            Go to admin
          </Button>
        )}
      </div>
    </div>
  )
}

export default SiderDashboard
