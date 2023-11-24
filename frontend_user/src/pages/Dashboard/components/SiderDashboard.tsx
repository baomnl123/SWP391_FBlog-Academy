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
  onGetSubjects?: (data: string[] | string | number | number[]) => void
  onGetMajors?: (data: string[] | string | number | number[]) => void
  onFilter?: (data: FilterType | null) => void
}

export type FilterType = 'image' | 'video'

const handleClick = () => {
  window.location.href = 'http://fblogadmin.netlify.app'
}

const SiderDashboard = ({ createPost, onGetSubjects, onGetMajors, onFilter }: SiderDashboardProps) => {
  const navigate = useNavigate()
  const [filter, setFilter] = useState<FilterType | null>(null)

  const userInfo = useSelector<RootState>((state) => state.userReducer.user)
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  const [_selectedMajor, setSelectedMajor] = useState<string | null>(null);
  const [_selectedSubject, setSelectedSubject] = useState<string | null>(null);

  const { data: majorsData } = useRequest(async () => {
    try {
      const res = await api.getAllMajor()
      return res.map((item) => {
        return {
          label: item.majorName,
          value: item.id
        }
      })
    } catch (error) {
      console.log(error)
    }
  })

  const { data: subjectsData } = useRequest(async () => {
    try {
      const res = await api.getAllSubject()
      return res.map((item) => {
        return {
          label: item.subjectName,
          value: item.id
        }
      })
    } catch (error) {
      console.log(error)
    }
  })

  

  return (
    <div>
      <div className='mb-6'>
      <SelectLabel
  label='Major'
  placeHolder='Select Major'
  optionData={majorsData}
  onChange={(value) => {
        setSelectedSubject(null); // Reset lựa chọn của Subject
        onGetMajors?.(typeof value === 'number' ? value.toString() : value);
        setFilter(null); // Reset lựa chọn của Image
        onFilter?.(null);
        return typeof value === 'number' ? value.toString() : value; // Cập nhật giá trị mới khi lựa chọn thay đổi
      }
    }
/>

<SelectLabel
  label='Subject'
  placeHolder='Select Subject'
  optionData={subjectsData}
  onChange={(value) => {
    
        setSelectedMajor(null); // Reset lựa chọn của Major
        onGetSubjects?.(typeof value === 'number' ? value.toString() : value);
        setFilter(null); // Reset lựa chọn của Image
        onFilter?.(null);
        return typeof value === 'number' ? value.toString() : value; // Cập nhật giá trị mới khi lựa chọn thay đổi
      }
    }

/>
      </div>
      <Flex
  align='center'
  className={`mb-8 mt-8 w-full cursor-pointer 
  ${filter === 'image' && !isDarkMode ? 'bg-[#C7DCF0]' : ''} 
  ${filter === 'image' && isDarkMode ? 'bg-[#0F2438]' : ''} 
  py-2 px-4 rounded-md`}
  gap={10}
  onClick={() => {
    if (filter !== 'image') {
      setFilter('image');
      onFilter?.('image');
    } else {
      setFilter(null); // Reset lựa chọn của Image và Video
      onFilter?.(null);
    }
  }}
>
        <IconPicture color={isDarkMode ? '#468CCE' : '#3178B9'} width={30} height={30} />
        <Typography.Text>Image</Typography.Text>
      </Flex>
      <Flex
        align='center'
        className={`mb-8 mt-8 w-full cursor-pointer 
        ${filter === 'video' && !isDarkMode ? 'bg-[#C7DCF0]' : ''} 
        ${filter === 'video' && isDarkMode ? 'bg-[#0F2438]' : ''} 
        py-2 px-4 rounded-md`}
        gap={10}
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
        <IconPhotoFilm color={isDarkMode ? '#468CCE' : '#3178B9'} width={30} height={30} />
        <Typography.Text>Video</Typography.Text>
      </Flex>
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
              onClick={() => navigate('/view-pending-list/')}
            >
              View pending list
            </Button>
            <Button
              className='w-full text-left mb-4'
              icon={<FileSyncOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
              size='large'
              onClick={() => navigate('/view-pending-post/')}
            >
              View My Pending Post
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
        {(userInfo as User)?.role === 'SU' && (
          <Button
            icon={<AuditOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
            size='large'
            className='w-full text-left mb-4'
            onClick={() => navigate('/view-pending-post')}
          >
            View My Pending Post
          </Button>
        )}
        {(userInfo as User)?.role !== 'AD' ? (
          <Button
            size='large'
            icon={<FileAddOutlined className={isDarkMode ? 'text-[#468CCE]' : 'text-[#3178B9]'} />}
            className='w-full text-left'
            onClick={() => createPost?.()}
          >
            Create Post
          </Button>
        ) : (
          <Button size='large' className='w-full text-left' onClick={() => handleClick()}>
            Go to admin
          </Button>
        )}
      </div>
    </div>
  )
}

export default SiderDashboard
