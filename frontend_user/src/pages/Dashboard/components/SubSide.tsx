import { RootState } from '@/store'
import { User } from '@/types'
import { AuditOutlined, FileDoneOutlined, FileSyncOutlined, GiftOutlined } from '@ant-design/icons'
import { Button } from 'antd'

import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'

export type FilterType = 'image' | 'video'

const SubSide = () => {
  const navigate = useNavigate()
  // const [categoryOptions, setCategoryOptions] = useState<SelectProps['options']>([])
  // const [tagOptions, setTagOptions] = useState<SelectProps['options']>([])
  const userInfo = useSelector<RootState>((state) => state.userReducer.user)
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)

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
