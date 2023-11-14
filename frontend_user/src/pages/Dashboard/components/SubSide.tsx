import api from '@/api'
import IconPhotoFilm from '@/assets/images/svg/IconPhotoFilm'
import IconPicture from '@/assets/images/svg/IconPicture'
import SelectLabel from '@/components/SelectLabel'
import { RootState } from '@/store'
import { User } from '@/types'
import { AuditOutlined, FileAddOutlined, FileDoneOutlined, FileSyncOutlined, GiftOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Button, Flex, Typography } from 'antd'
import { useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'

interface SiderDashboardProps {
  createPost?: () => void
  onGetTags?: (data: string[] | string | number | number[]) => void
  onGetCategories?: (data: string[] | string | number | number[]) => void
  onFilter?: (data: FilterType | null) => void
}

export type FilterType = 'image' | 'video'

const SubSide = () => {
  const navigate = useNavigate()
  const [filter, setFilter] = useState<FilterType | null>(null)
  // const [categoryOptions, setCategoryOptions] = useState<SelectProps['options']>([])
  // const [tagOptions, setTagOptions] = useState<SelectProps['options']>([])
  const userInfo = useSelector<RootState>((state) => state.userReducer.user)
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  const [categories, setCategories] = useState<string | string[] | number | number[] | null>([])
  const { data: categoriesData } = useRequest(async () => {
    try {
      const res = await api.getAllCategory()
      return res.map((item) => {
        return {
          label: item.categoryName,
          value: item.id
        }
      })
    } catch (error) {
      console.log(error)
    }
  })

  const { data: tagsData } = useRequest(async () => {
    try {
      const res = await api.getAllTag()
      return res.map((item) => {
        return {
          label: item.tagName,
          value: item.id
        }
      })
    } catch (error) {
      console.log(error)
    }
  })

  return (
    <div>
      <div className='mb-8'>
        {(userInfo as User)?.role === 'LT' && (
          <Button
            icon={<AuditOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
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
            icon={<FileDoneOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
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
              icon={<FileSyncOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
              size='large'
              onClick={() => navigate('/view-pending-list')}
            >
              View pending list
            </Button>
            <Button
              size='large'
              className='w-full text-left mb-4'
              icon={<GiftOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
              onClick={() => navigate('/give-awards')}
            >
              Give awards
            </Button>
          </>
        )}
      </div>
    </div>
  )
}

export default SubSide