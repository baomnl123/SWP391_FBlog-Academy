import api from '@/api'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Button, Modal, Radio, Space, Typography, message } from 'antd'
import { useCallback, useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import ModalCreateSaveList from './ModalCreateSaveList'

type IModalSave = {
  open: boolean
  onClose: () => void
  postId: number
}

export default function ModalSave({ open, onClose, postId }: IModalSave) {
  const [openCreate, setOpenCreate] = useState(false)
  const [defaultSave, setDefaultSave] = useState(0)
  const id = useSelector<RootState>((state) => state.userReducer.user?.id)

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

  const save = useCallback(
    async (saveListId: number) => {
      try {
        await api.savePost(saveListId, postId)
        message.success('Saved post')
      } catch (e) {
        console.error(e)
      }
    },
    [postId]
  )

  useEffect(() => {
    if (open) {
      getSaveList()
    }
  }, [open, getSaveList])

  return (
    <>
      <Modal
        title='Save list'
        open={open}
        onCancel={() => {
          onClose()
        }}
        onOk={() => {
          save(defaultSave)
          onClose()
        }}
        okText='Save'
      >
        <div className='mt-5'>
          <div className='text-right'>
            <Button
              onClick={() => {
                setOpenCreate(true)
              }}
            >
              Create save list
            </Button>
          </div>
          {data ? (
            <Radio.Group value={defaultSave} onChange={(e) => setDefaultSave(e.target.value)}>
              <Space size={10} direction='vertical'>
                {data?.map((saveList) => (
                  <Radio key={saveList.id} value={saveList.id}>
                    {saveList.name}
                  </Radio>
                ))}
              </Space>
            </Radio.Group>
          ) : (
            <Typography.Text className='my-5 text-center'>Empty list</Typography.Text>
          )}
        </div>
      </Modal>
      <ModalCreateSaveList
        open={openCreate}
        onOk={() => {
          refresh()
        }}
        onClose={() => {
          setOpenCreate(false)
        }}
      />
    </>
  )
}
