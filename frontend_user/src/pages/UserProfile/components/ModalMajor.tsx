import api from '@/api'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Form, Modal, Spin } from 'antd'

import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import SelectLabel from '@/components/SelectLabel'

interface ModalMajorProps {
  isOpen: boolean
  setModal?: (value: boolean) => void
  idPost?: number | null
  onSuccess?: () => void
  onGetMajors?: (data: string[] | string | number | number[]) => void
  onOk?: () => void
  onCancel?: () => void
  majorSelect?: any[]
}

const ModalMajor = ({ isOpen, setModal, onSuccess, onOk, majorSelect }: ModalMajorProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [major, setMajor] = useState<any[]>()
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)

  const { data: majorsData } = useRequest(async () => {
    try {
      const res = await api.getAllMajor()
      // Filter all major, expect Only Student
      const filter = res.filter((item) => item.majorName !== 'Only Students')

      return filter.map((item) => {
        return {
          label: item.majorName,
          value: item.id
        }
      })
    } catch (error) {
      console.log(error)
    }
  })

  useEffect(() => {
    setIsModalOpen(isOpen)
  }, [isOpen])

  const handleOk = async () => {
    try {
      if ((majorSelect ?? []).length > 0) {
        if (major?.length ?? 0 > 0) {
          // Delete all unselected majors
          const majorList = majorSelect?.filter((item) => !major?.includes(item))
          await api.deleteUserMajor(user?.id ?? 0, majorList ?? [])
        } else {
          await api.deleteUserMajor(user?.id ?? 0, majorSelect ?? [])
        }
      }
      if (major?.length ?? 0 > 0) {
        await api.createdUserMajor({
          majorID: major ?? [],
          userID: user?.id ?? 0
        })
      }
      onOk?.()
    } catch (e) {
      console.error(e)
    }
  }

  const handleCancel = () => {
    form.resetFields()
    onSuccess?.()
    setIsModalOpen(false)
    setModal?.(false)
    setMajor([])
  }

  useEffect(() => {
    setMajor(majorSelect)
  }, [majorSelect])

  return (
    <Spin>
      <Modal open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
        <SelectLabel
          label='Major'
          placeHolder='Select Major'
          optionData={majorsData}
          onChange={(value) => {
            setMajor(value as number[])
          }}
          value={major}
        />
      </Modal>
    </Spin>
  )
}

export default ModalMajor
