import { Categories, Lecturers, ReportPost, Tag, UserEmail } from '@/types'
import axiosClient from './axios'

const api = {
  // category
  getCategories() {
    const url = 'Category/all'
    return axiosClient.get<unknown, Categories[]>(url)
  },
  createCategory(adminId: number, payload: FormData) {
    const url = `Category?adminId=${adminId}`
    return axiosClient.post(url, payload)
  },
  updateCategory(categoryId: number, payload: FormData) {
    const url = `Category/${categoryId}`
    return axiosClient.put(url, payload)
  },
  deleteCategory(id: number) {
    const url = `Category/${id}`
    return axiosClient.delete(url)
  },

  // tag
  getTagByCategory(id: number) {
    const url = `Category/${id}/tags`
    return axiosClient.get<unknown, Tag[]>(url)
  },
  getTags() {
    const url = 'Tag/all'
    return axiosClient.get<unknown, Tag[]>(url)
  },
  getTagById(tagId: number) {
    const url = `Tag/${tagId}`
    return axiosClient.get(url)
  },
  deleteTag(id: number) {
    const url = `Tag/${id}`
    return axiosClient.delete(url)
  },
  deleteTagFromCategory(categoryId: number, tagId: number) {
    const url = `Tag/${categoryId}/${tagId}`
    return axiosClient.delete(url)
  },
  createTag(adminId: number, categoryId: number, payload: FormData) {
    const url = `Tag?adminId=${adminId}&categoryId=${categoryId}`
    return axiosClient.post(url, payload)
  },
  updateTag(tagId: number, payload: FormData) {
    const url = `Tag/${tagId}`
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
  }
}

export default api
