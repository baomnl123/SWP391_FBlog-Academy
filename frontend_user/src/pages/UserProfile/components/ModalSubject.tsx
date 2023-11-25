import api from '@/api'
import { RootState } from '@/store'
import { useRequest } from 'ahooks'
import { Form, Modal, Spin } from 'antd'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import SelectLabel from '@/components/SelectLabel'

interface ModalSubjectProps {
  isOpen: boolean
  setModal?: (value: boolean) => void
  idPost?: number | null
  majorIds?: number | number[] | undefined
  onSuccess?: () => void
  onOk?: () => void
  onCancel?: () => void
  subjectSelect?: any[]
}

const ModalSubject = ({ isOpen, setModal, majorIds, onSuccess, onOk, subjectSelect }: ModalSubjectProps) => {
  const [isModalOpen, setIsModalOpen] = useState(isOpen)
  const [subject, setSubject] = useState<any[]>([])
  const [optionDatas, setOptionDatas] = useState<any[]>([])
  const [form] = Form.useForm()
  const { user } = useSelector((state: RootState) => state.userReducer)
  console.log(subject)

  const { data: subjectsData } = useRequest(async () => {
    try {
      const res = await api.getAllMajor()
      return res
    } catch (error) {
      console.log(error)
      return []
    }
  })

  useEffect(() => {
    // Filter the data when majorIds changes
    if (subjectsData && subjectsData.length > 0) {
      const filteredSubjects = subjectsData
        .filter((major) => (Array.isArray(majorIds) ? majorIds.includes(major.id) : major.id === majorIds))
        .flatMap((major) =>
          major.subjects.map((item) => ({
            label: item.subjectName,
            value: item.id
          }))
        )

      setOptionDatas(filteredSubjects)
    }
  }, [subjectsData, majorIds])

  useEffect(() => {
    setIsModalOpen(isOpen)
  }, [isOpen])

  const handleOk = async () => {
    try {
      if ((subjectSelect ?? []).length > 0) {
        await api.deleteUserSubject(user?.id ?? 0, subjectSelect ?? [])
      }
      if (subject.length > 0) {
        await api.createdUserSubject({
          subjectID: subject,
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
    setSubject([])
  }

  useEffect(() => {
    if (subjectSelect !== undefined) {
      setSubject(subjectSelect);
    }
  }, [subjectSelect]);

  return (
    <Spin>
      <Modal open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
        <SelectLabel
          label='Subject'
          placeHolder='Select Subject'
          optionData={optionDatas}
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