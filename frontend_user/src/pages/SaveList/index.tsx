import api from '@/api'
import BaseLayout from '@/components/BaseLayout'
import { RootState } from '@/store'
import { DeleteOutlined, EditOutlined } from '@ant-design/icons'
import { useRequest } from 'ahooks'
import { Button, Card, Flex, List, Modal, Space, Typography, message } from 'antd'
import { useCallback, useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import ModalCreateSaveList from './ModalCreateSaveList'
import SubSide from '../Dashboard/components/SubSide'

export default function SaveList() {
  const navigate = useNavigate()
  const id = useSelector<RootState>((state) => state.userReducer.user?.id)
  const [modal, contextHolder] = Modal.useModal()
  const [open, setOpen] = useState(false)
  const [saveList, setSaveList] = useState<{ id: number; name: string } | undefined>()

  const {
    data,
    run: getSaveList,
    refresh
  } = useRequest(
    async () => {
      const response = await api.saveList(id as number)
      return response
    },
    {
      manual: true,
      onError(e) {
        console.error(e)
      }
    }
  )

  const deleteSaveList = useCallback(
    async (id: number) => {
      try {
        await api.deleteSaveList(id)
        refresh()
        message.success('Delete save list success')
      } catch (e) {
        console.error(e)
      }
    },
    [refresh]
  )

  useEffect(() => {
    if (id) {
      getSaveList()
    }
  }, [getSaveList, id])

  return (
    <BaseLayout
      sider={
        <Flex justify='space-between' align='center' vertical className='h-full w-full'>
          <SubSide />
          <Button size='large' block type='primary' onClick={() => setOpen(true)}>
            Create Save List
          </Button>
        </Flex>
      }
    >
      <Card className='w-[800px] mx-auto'>
        <List
          dataSource={data}
          renderItem={(item) => (
            <List.Item key={item.id} className='w-full'>
              <Flex
                justify='space-between'
                align='center'
                className='cursor-pointer w-full'
                onClick={() => navigate(`${item.id}`)}
              >
                <Typography.Text className='text-xl py-5'>{item.name}</Typography.Text>
                <Space size={10}>
                  <Button
                    icon={<EditOutlined />}
                    onClick={(e) => {
                      e.stopPropagation()
                      setSaveList({
                        id: item.id ?? 0,
                        name: item.name ?? ''
                      })
                      setOpen(true)
                    }}
                  />
                  <Button
                    danger
                    icon={<DeleteOutlined />}
                    onClick={(e) => {
                      e.stopPropagation()
                      modal.confirm({
                        title: 'Delete save list',
                        content: 'Do you want to delete this save list?',
                        async onOk() {
                          await deleteSaveList(item?.id ?? 0)
                        }
                      })
                    }}
                  />
                </Space>
              </Flex>
            </List.Item>
          )}
        />
      </Card>
      <ModalCreateSaveList
        open={open}
        saveList={saveList}
        onOk={() => {
          refresh()
          setOpen(false)
          setSaveList(undefined)
        }}
        onClose={() => {
          setOpen(false)
          setSaveList(undefined)
        }}
      />
      {contextHolder}
    </BaseLayout>
  )
}
