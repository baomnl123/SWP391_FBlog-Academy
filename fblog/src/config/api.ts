import { Major, Lecturers, ReportPost, Subject, UserEmail, Post } from '@/types'
import axiosClient from './axios'

const api = {
  // major
  getAllMajor() {
    const url = 'Major/all'
    return axiosClient.get<unknown, Major[]>(url)
  },
  createMajor(adminId: number, payload: FormData) {
    const url = `Major?adminId=${adminId}`
    return axiosClient.post(url, payload)
  },
  updateMajor(majorId: number, payload: FormData) {
    const url = `Major/${majorId}`
    return axiosClient.put(url, payload)
  },
  deleteMajor(id: number) {
    const url = `Major/${id}`
    return axiosClient.delete(url)
  },

  // subject
  getSubjectByMajor(id: number) {
    const url = `Major/${id}/subjects`
    return axiosClient.get<unknown, Subject[]>(url)
  },
  getAllSubject() {
    const url = 'Subject/all'
    return axiosClient.get<unknown, Subject[]>(url)
  },
  getSubjectById(subjectId: number) {
    const url = `Subject/${subjectId}`
    return axiosClient.get(url)
  },
  deleteSubject(id: number) {
    const url = `Subject/${id}`
    return axiosClient.delete(url)
  },
  deleteSubjectFromMajor(majorId: number, subjectId: number) {
    const url = `Subject/${majorId}/${subjectId}`
    return axiosClient.delete(url)
  },
  createSubject(adminId: number, majorId: number, payload: FormData) {
    const url = `Subject?adminId=${adminId}&majorId=${majorId}`
    return axiosClient.post(url, payload)
  },
  updateSubject(subjectId: number, payload: FormData) {
    const url = `Subject/${subjectId}`
    return axiosClient.put(url, payload)
  },

  // user
  getEmail(email: string) {
    const url = `User/email/${email}`
    return axiosClient.get<unknown, UserEmail>(url)
  },

  // lecturers
  getLecturers() {
    const url = 'User/all/lecturers'
    return axiosClient.get<unknown, Lecturers[]>(url)
  },

  createLectures({ name, email, password }: { name: string; email: string; password: string }) {
    const url = 'User/lecturer'
    const formData = new FormData()
    formData.append('name', name)
    formData.append('email', email)
    formData.append('password', password)
    return axiosClient.post(url, formData)
  },

  deleteLectures(userId: string) {
    const url = 'User/' + userId
    return axiosClient.delete(url)
  },

  banStudent(id: number) {
    const url = 'User/' + id
    return axiosClient.delete(url)
  },

  unbanStudent(id: number) {
    const url = `User/${id}/unban`
    return axiosClient.post(url)
  },

  getStudent() {
    const url = 'User/students-and-moderators'
    return axiosClient.get<unknown, UserEmail[]>(url)
  },

  getStudentUnban() {
    const url = 'User/banned'
    return axiosClient.get<unknown, UserEmail[]>(url)
  },

  // post
  reportPostPending() {
    const url = 'ReportPost/pending'
    return axiosClient.get<unknown, ReportPost[]>(url)
  },

  approvePost(reporterID: number, postID: number) {
    const url = 'ReportPost/status'
    return axiosClient.put(url, null, {
      params: {
        adminID: Number(localStorage.getItem('id')),
        reporterID,
        postID
      }
    })
  },

  denyPost(reporterID: number, postID: number) {
    const url = 'ReportPost'
    return axiosClient.delete(url, {
      params: {
        adminID: Number(localStorage.getItem('id')),
        reporterID,
        postID
      }
    })
  },

  reportPost() {
    const url = 'ReportPost'
    return axiosClient.get<unknown, ReportPost[]>(url)
  },

  getAllPost() {
    const url = `Post/all/1`
    return axiosClient.get<unknown, Post[]>(url)
  },

  getPostById(postId: number) {
    const url = `Post/${postId}`
    return axiosClient.get<unknown, Post>(url, {
      params: {
        currentUserId: Number(localStorage.getItem('id'))
      }
    })
  },

  deletPost(postId: number) {
    const url = `Post/${postId}`
    return axiosClient.delete(url)
  }
}

export default api
