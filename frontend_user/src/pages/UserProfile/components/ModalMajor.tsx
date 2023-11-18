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
  userId?: number | null
  onSuccess?: () => void
  onGetCategories?: (data: string[] | string | number | number[]) => void
}

const ModalMajor = ({ isOpen, setModal, userId, onSuccess, onGetCategories }: ModalMajorProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)
  

  const { runAsync: sendReport, loading: reportLoading } = useRequest(api.createUserMajor, {
    manual: true,
    onSuccess: (res) => {
      if (res) {
        message.success('Create User Success')
        onSuccess?.()
        form.resetFields()
        setIsModalOpen(false)
        setModal?.(false)
      }
    },
    onError: (err) => {
      console.log(err)
    }
  })
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

  useEffect(() => {
    setIsModalOpen(isOpen)
  }, [isOpen])

  const handleOk = () => {
    form.submit()
  }

  const handleCancel = () => {
    form.resetFields()
    onSuccess?.()
    setIsModalOpen(false)
    setModal?.(false)
  }

  const onFinish = async (value: { majorId: number, userId:number }) => {
    await sendReport({
      content: value.content,
      postID: idPost ?? 0,
      reporterID: user?.id ?? 0
    })
  }
  

  return (
    <Spin spinning={reportLoading}>
      <Modal open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
        <SelectLabel
          label='Major'
          placeHolder='Select Major'
          optionData={categoriesData}
          onChange={(value) => {
            onGetCategories?.(value)
          }}
        />
      </Modal>
    </Spin>
  )
}

export default ModalMajor