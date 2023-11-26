import IconMoon from '@/assets/images/svg/IconMoon'
import IconSun from '@/assets/images/svg/IconSun'
import { getLocalStorage, removeLocalStorage, setLocalStorage } from '@/utils/helpers'
import {
  AppstoreOutlined,
  CodeSandboxOutlined,
  FileOutlined,
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  TagsOutlined,
  TeamOutlined,
  UserOutlined,
  SafetyOutlined,
  FormOutlined
} from '@ant-design/icons'
import { UserButton } from '@clerk/clerk-react'
import type { MenuProps } from 'antd'
import { Breadcrumb, ConfigProvider, Flex, FloatButton, Layout, Menu, theme } from 'antd'
import { PropsWithChildren, useEffect, useState } from 'react'
import { Link, RouteObject, useLocation, useMatches } from 'react-router-dom'

const { Header, Content, Footer, Sider } = Layout

type MenuItem = Required<MenuProps>['items'][number]

function getItem(label: React.ReactNode, key: React.Key, icon?: React.ReactNode, children?: MenuItem[]): MenuItem {
  return {
    key,
    icon,
    children,
    label
  } as MenuItem
}

const items: MenuItem[] = [
  getItem(<Link to='/'>Dashboard</Link>, '/dashboard', <AppstoreOutlined />),
  getItem(<Link to='/major'>Major</Link>, '/major', <CodeSandboxOutlined />),
  getItem(<Link to='/subject'>Subject</Link>, '/subject', <TagsOutlined />),
  getItem(<Link to='/lecturer'>Lecturer</Link>, '/lecturer', <UserOutlined />),
  getItem(<Link to='/student'>Student</Link>, '/student', <TeamOutlined />),
  getItem(<Link to='/report'>Report</Link>, '/report', <FileOutlined />),
  getItem(<Link to='/post'>Post</Link>, '/post', <FormOutlined />),
  getItem(<Link to='/policy'>Policy</Link>, '/policy', <SafetyOutlined />)
]

const BaseLayout = ({ children }: PropsWithChildren) => {
  const [pathActive, setPathActive] = useState(['/dashboard'])
  const location = useLocation()

  useEffect(() => {
    setPathActive([location.pathname])
  }, [location])

  const [collapsed, setCollapsed] = useState(false)
  const [isDarkMode, setIsDarkMode] = useState(false)

  const { defaultAlgorithm, darkAlgorithm } = theme

  const matches: RouteObject[] = useMatches()
  const crumbs = matches
    .filter((match) => Boolean(match.handle?.crumb))
    .map((match) => match.handle?.crumb(match.handle.data))

  const handleClick = () => {
    if (isDarkMode) {
      removeLocalStorage('dark-mode')
    } else {
      setLocalStorage('dark-mode', true)
    }
    setIsDarkMode((previousValue) => !previousValue)
  }

  useEffect(() => {
    const darkMode = getLocalStorage('dark-mode')
    if (!darkMode) {
      setIsDarkMode(false)
      return
    }
    setIsDarkMode(true)
  }, [])

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
        <Sider
          collapsible
          reverseArrow
          collapsed={collapsed}
          onCollapse={(value) => setCollapsed(value)}
          theme={isDarkMode ? 'dark' : 'light'}
          className={`shadow-md ${!collapsed ? 'fixed-sidebar' : ''}`}
          trigger={
            <>
              <div className={`px-5 border-t ${collapsed ? 'text-center' : 'text-right'}`}>
                {collapsed ? (
                  <MenuUnfoldOutlined width={50} height={50} />
                ) : (
                  <MenuFoldOutlined width={50} height={50} />
                )}
              </div>
            </>
          }
        >
          <div className='flex justify-center items-center'>
            <Link to='/dashboard'>
              <img
                src={`/${collapsed ? 'sm-logo.png' : 'logo.png'}`}
                className={`${collapsed && 'py-5'}`}
                alt=''
                width={collapsed ? 20 : 100}
              />
            </Link>
          </div>
          <Menu
            theme={isDarkMode ? 'dark' : 'light'}
            mode='inline'
            items={items}
            defaultSelectedKeys={['/dashboard']}
            defaultOpenKeys={['/dashboard']}
            selectedKeys={pathActive}
          />
        </Sider>
        <Layout>
          <Header style={{ padding: 0, background: isDarkMode ? '#141414' : '#fff' }} className='shadow-md'>
            <Flex justify='end' align='center' className='h-full px-10'>
              {/* <Dropdown menu={{ items: itemsMenu }} arrow placement='bottomRight'>
                <Space size={10} align='center' className='cursor-pointer'>
                  <Avatar src={user?.imageUrl} />
                  <Typography>{user?.fullName}</Typography>
                </Space>
              </Dropdown> */}
              <UserButton />
            </Flex>
          </Header>
          <Content style={{ margin: '0 16px' }}>
            <Breadcrumb
              style={{ margin: '16px 0' }}
              items={crumbs.map((crumb) => ({
                title: crumb
              }))}
            />
            <FloatButton
              className='min-h-[unset] w-10 h-10 m-auto text-center rounded-[50%] fixed bottom-10 right-10 p-0'
              onClick={handleClick}
              icon={isDarkMode ? <IconSun /> : <IconMoon />}
            />
            {children}
          </Content>
          <Footer style={{ textAlign: 'center' }}>Fblog ©2023 Created by FPTU</Footer>
        </Layout>
      </Layout>
    </ConfigProvider>
  )
}

export default BaseLayout
