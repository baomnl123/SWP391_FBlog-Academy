import api from '@/api'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Form, Modal, Spin, Typography, message } from 'antd'
import TextArea from 'antd/es/input/TextArea'
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
  majorSelect?: any[]
}

const ModalMajor = ({ isOpen, setModal, onSuccess, onOk, majorSelect }: ModalMajorProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [major, setMajor] = useState<any[]>()
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)

  const { data: categoriesData } = useRequest(async () => {
    try {
      const res = await api.getAllCategory()
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
          optionData={categoriesData}
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
