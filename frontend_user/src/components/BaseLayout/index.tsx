import { RootState } from '@/store'
import { setDarkMode } from '@/store/reducers/theme'
import { getLocalStorage, removeLocalStorage, setLocalStorage } from '@/utils/helpers'
import { SearchOutlined } from '@ant-design/icons'
import { UserButton } from '@clerk/clerk-react'
import { Breadcrumb, ConfigProvider, Flex, Input, Layout, Space, Switch, Typography, theme } from 'antd'
import { PropsWithChildren, ReactNode, useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { RouteObject, useMatches, useNavigate } from 'react-router-dom'

const { Header, Content, Sider } = Layout

const BaseLayout = ({
  children,
  sider,
  rightSider,
  showSearch = false,
  onChangeSearch
}: PropsWithChildren<{
  sider: ReactNode
  rightSider?: ReactNode
  showSearch?: boolean
  onChangeSearch?: (value: string) => void
}>) => {
  // const [isDarkMode, setIsDarkMode] = useState(false)
  const navigate = useNavigate()
  const { defaultAlgorithm, darkAlgorithm } = theme
  const isDarkMode = useSelector((state: RootState) => state.themeReducer.darkMode)
  const dispatch = useDispatch()

  const matches: RouteObject[] = useMatches()
  const crumbs = matches
    .filter((match) => Boolean(match.handle?.crumb))
    .map((match) => match.handle?.crumb(match.handle.data))

  const handleClick = (value: boolean) => {
    if (isDarkMode) {
      removeLocalStorage('dark-mode')
    } else {
      setLocalStorage('dark-mode', true)
    }
    dispatch(setDarkMode(value))
  }

  useEffect(() => {
    const darkMode = getLocalStorage('dark-mode')
    if (!darkMode) {
      dispatch(setDarkMode(false))
      return
    }
    dispatch(setDarkMode(true))
  }, [dispatch])

  return (
    <ConfigProvider
      theme={{
        algorithm: isDarkMode ? darkAlgorithm : defaultAlgorithm,
        components: {
          Layout: {
            siderBg: '#141414',
            triggerBg: '#141414',
            bodyBg: isDarkMode ? 'rgb(42, 44, 44)' : '#f5f5f5',
            footerBg: isDarkMode ? 'rgb(42, 44, 44)' : '#f5f5f5'
          },
          Menu: {
            darkItemBg: '#141414'
          },
          Table: {
            colorBgContainer: isDarkMode ? '#262626' : '#FFFFFF',
            headerBg: isDarkMode ? '#434343' : '#FAFAFA'
          }
        }
      }}
    >
      <Layout style={{ minHeight: '100vh' }} className={isDarkMode ? 'dark' : ''}>
        <Header style={{ padding: 0, background: isDarkMode ? '#141414' : '#fff' }} className='fixed w-full z-50'>
          <Flex align='center' className='w-full px-5 shadow-md' gap={190}>
            <div className='cursor-pointer' onClick={() => navigate('/')}>
              <Typography.Title level={1}>Logo</Typography.Title>
            </div>
            {showSearch && (
              <Input
                prefix={<SearchOutlined />}
                className='w-[1300px]'
                onChange={(e) => {
                  onChangeSearch?.(e.target.value)
                }}
              />
            )}
          </Flex>
        </Header>
        <Layout>
          <Sider width={300} theme={isDarkMode ? 'dark' : 'light'} className='shadow-md fixed-sidebar'>
            <Flex justify='space-between' vertical className='h-full w-full pb-5'>
              <Flex gap={10} vertical className='w-full px-5 grow mb-5'>
                {sider}
              </Flex>
              <Flex justify='space-between' align='center' className='mr-5'>
                <Space size={10} align='center' className='cursor-pointer px-5 flex-none'>
                  <UserButton />
                  <Typography.Text>Profile</Typography.Text>
                </Space>
                <Switch onChange={(e) => handleClick(e)} className='ml-5' checked={!!isDarkMode} />
              </Flex>
            </Flex>
          </Sider>
          <Layout style={{ padding: '65px 0 24px 24px' }}>
            <Breadcrumb style={{ margin: '16px 0' }}>
              {crumbs.map((crumb, index) => (
                <Breadcrumb.Item key={index}>
                  <Typography.Title level={4}>{crumb}</Typography.Title>
                </Breadcrumb.Item>
              ))}
            </Breadcrumb>
            <Content style={{ margin: '0 16px' }}>{children}</Content>
          </Layout>
          {rightSider && (
            <Sider width={300} theme={isDarkMode ? 'dark' : 'light'} className='shadow-md'>
              {rightSider}
            </Sider>
          )}
        </Layout>
      </Layout>
    </ConfigProvider>
  )
}

export default BaseLayout
