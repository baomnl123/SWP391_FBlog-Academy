// import axiosClient from './axiosClient'

import { Category, PendingPost, PostByUserId, SavePost, Tag, User, Post } from '@/types'
import axiosClient from './axiosClient'
import { CreatePostBodyRequest } from './types/post'

const api = {
  // post
  postPending() {
    const url = 'Post/pending'
    return axiosClient.get<unknown, PendingPost[]>(url)
  },
  postApproved() {
    const url = 'Post/all'
    return axiosClient.get<unknown, PendingPost[]>(url)
  },

  postCategoryTag({
    categoryID,
    tagID,
    currentUserId
  }: {
    categoryID?: number[]
    tagID?: number[]
    currentUserId?: number
  }) {
    const url = 'Post/category-tag'
    return axiosClient.get<unknown, PendingPost[]>(url, {
      params: {
        categoryID,
        tagID,
        currentUserId
      }
    })
  },

  approvePost(reviewerId: number, postId: number) {
    const url = 'Post/approve'
    return axiosClient.put(url, null, {
      params: {
        reviewerId,
        postId
      }
    })
  },

  denyPost(reviewerId: number, postId: number) {
    const url = 'Post/deny'
    return axiosClient.put(url, null, {
      params: {
        reviewerId,
        postId
      }
    })
  },

  getPostByUserId(userId: number) {
    const url = `Post/user/${userId}`
    return axiosClient.get<unknown, PostByUserId[]>(url)
  },

  //category
  getAllCategory() {
    const url = 'Category/all'
    return axiosClient.get<unknown, Category[]>(url)
  },

  deletePost(postId: number) {
    const url = `Post/${postId}`
    return axiosClient.delete(url)
  },

  //tag
  getAllTag() {
    const url = 'Tag/all'
    return axiosClient.get<unknown, Tag[]>(url)
  },

  filterCategoryTag(categoryID?: number[], tagID?: number[], searchValue?: string) {
    const url = `Post/category-tag`
    return axiosClient.get(url, {
      params: {
        categoryID,
        tagID,
        searchValue
      }
    })
  },

  //user
  getUserById(id: number) {
    const url = `User/${id}`
    return axiosClient.get<unknown, User>(url)
  },

  getStudentAndModerator() {
    const url = 'User/students-and-moderators'
    return axiosClient.get<unknown, User[]>(url)
  },

  giveAward(userId: number) {
    const url = `User/${userId}/award`
    return axiosClient.put(url)
  },

  removeAward(userId: number) {
    const url = `User/${userId}/award`
    return axiosClient.delete(url)
  },

  followerByUserId(currentUserID: number) {
    const url = `User/${currentUserID}/follower`
    return axiosClient.get<unknown, User[]>(url)
  },

  followingByUserId(currentUserID: number) {
    const url = `User/${currentUserID}/following`
    return axiosClient.get<unknown, User[]>(url)
  },

  createNewAccount(payload: FormData) {
    const url = 'User/student'
    return axiosClient.post(url, payload)
  },

  //post
  createPost({ body, id }: { body: CreatePostBodyRequest; id: number }) {
    const url = 'Post'
    const { categoryIds, content, imageURLs, tagIds, title, videoURLs } = body
    const formData = new FormData()
    formData.append('title', title)
    formData.append('content', content)

    return axiosClient.post(url, formData, {
      params: {
        userId: id,
        categoryIds,
        imageURLs,
        tagIds,
        videoURLs
      }
    })
  },

  reportPost({ reporterID, postID, content }: { reporterID: number; postID: number; content: string }) {
    const url = 'ReportPost'
    const formData = new FormData()
    formData.append('content', content)
    return axiosClient.post(url, formData, {
      params: {
        reporterID,
        postID
      }
    })
  },

  // get user by email
  getUserByEmail({ email }: { email: string }) {
    const url = 'User/email/' + email
    return axiosClient.get(url)
  },

  getCommentByPost({ postId }: { postId: number }) {
    const url = `Comment/${postId}/comments`
    return axiosClient.get(url)
  },

  createCommentByPost({ postId, userId, content }: { postId: number; userId: number; content: string }) {
    const url = `Comment`
    const formData = new FormData()
    formData.append('content', content)

    return axiosClient.post(url, formData, {
      params: {
        userId,
        postId
      }
    })
  },

  promote(id: number) {
    const url = `User/${id}/promote`
    return axiosClient.put(url)
  },

  demote(id: number) {
    const url = `User/${id}/demote`
    return axiosClient.put(url)
  },

  // save post
  savePost(saveListID: number, postID: number) {
    const url = 'Post/saved-post'
    return axiosClient.post(url, null, {
      params: {
        saveListID,
        postID
      }
    })
  },

  createSaveList(userID: number, payload: FormData) {
    const url = 'SaveList'
    return axiosClient.post(url, payload, {
      params: {
        userID
      }
    })
  },

  getPostById({ postId }: { postId: number }) {
    const url = 'Post/' + postId
    return axiosClient.get<unknown, Post>(url)
  },

  updatePost({
    postId,
    imageURLs,
    videoURLs,
    categoryIds,
    tagIds,
    content,
    title
  }: {
    postId: number
    categoryIds?: number[]
    tagIds?: number[]
    videoURLs: string[]
    imageURLs: string[]
    title: string
    content: string
  }) {
    const url = 'Post?postId=' + postId
    const formData = new FormData()
    formData.append('title', title)
    formData.append('content', content)
    return axiosClient.put(url, formData, {
      params: {
        imageURLs,
        videoURLs,
        categoryIds,
        tagIds
      }
    })
  },

  saveList(userId: number) {
    const url = `SaveList/${userId}`
    return axiosClient.get<unknown, SavePost[]>(url)
  },

  postSaved(saveListID: number) {
    const url = `SaveList/${saveListID}/posts`
    return axiosClient.get<unknown, PendingPost[]>(url)
  },

  deletePostFromSaveList(saveListID: number, postID: number) {
    const url = 'SaveList'
    return axiosClient.delete(url, {
      params: {
        saveListID,
        postID
      }
    })
  },
  deleteSaveList(saveListID: number) {
    const url = `SaveList/${saveListID}`
    return axiosClient.delete(url)
  },
  updateSaveList(saveListID: number, payload: FormData) {
    const url = `SaveList/${saveListID}`
    return axiosClient.put(url, payload)
  },

  votePost({ currentUserId, postId, vote }: { currentUserId: number; postId: number; vote: string }) {
    const url = 'VotePost'
    const formData = new FormData()
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    formData.append('vote', vote as any)
    return axiosClient.post(url, formData, {
      params: {
        currentUserId,
        postId
      }
    })
  },

  voteUpdate({ currentUserId, postId, vote }: { currentUserId: number; postId: number; vote: string }) {
    const url = 'VotePost'
    const formData = new FormData()
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    formData.append('vote', vote as any)
    return axiosClient.put(url, formData, {
      params: {
        currentUserId,
        postId
      }
    })
  },

  allPostHasImages() {
    const url = 'Post/all-post-has-image'
    return axiosClient.get<unknown, PendingPost[]>(url)
  },

  allPostHasVideo() {
    const url = 'Post/all-post-has-video'
    return axiosClient.get<unknown, PendingPost[]>(url)
  }
}

export default api
