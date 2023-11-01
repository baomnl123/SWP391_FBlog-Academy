import { Categories, Tag, UserEmail } from '@/types'
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
  deleteTag(id: number) {
    const url = `Tag/${id}`
    return axiosClient.delete(url)
  },
  deleteTagFromCategory(categoryId: number, tagId: number) {
    const url = `Tag/${categoryId}/${tagId}`
    return axiosClient.delete(url)
  },
  createTag(payload: FormData) {
    const url = 'Tag'
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
  }
}

export default api
