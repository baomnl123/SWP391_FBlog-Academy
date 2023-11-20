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
  onGetCategories?: (data: string[] | string | number | number[]) => void
  onOk?: () => void
  onCancel?: () => void
  subjectSelect?: any[]
}

const ModalSubject = ({ isOpen, setModal, onSuccess, onOk, subjectSelect }: ModalMajorProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [subject, setSubject] = useState<any[]>()
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)

  const { data: tagsData } = useRequest(async () => {
    try {
      const res = await api.getAllTag()
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

  useEffect(() => {
    setIsModalOpen(isOpen)
  }, [isOpen])

  const handleOk = async () => {
    try {
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
    setSubject([])
  }

  useEffect(() => {
    setSubject(subjectSelect)
  }, [subjectSelect])

  return (
    <Spin>
      <Modal open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
        <SelectLabel
          label='Major'
          placeHolder='Select Major'
          optionData={tagsData}
          onChange={(value) => {
            setSubject(value as number[])
          }}
          value={subject}
        />
      </Modal>
    </Spin>
  )
}

export default ModalSubject
