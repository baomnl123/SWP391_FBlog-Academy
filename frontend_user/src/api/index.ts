// import axiosClient from './axiosClient'

import { Category, PendingPost, PostByUserId, SavePost, Tag, User, Post } from '@/types'
import axiosClient from './axiosClient'
import { CreatePostBodyRequest } from './types/post'
import { UserMajor, UserSubject } from './types/user'
import axios from 'axios'

const api = {
  // post
  postPending({
    categoryID,
    tagID,
    currentUserId,
    searchValue
  }: {
    categoryID?: number[]
    tagID?: number[]
    currentUserId?: number
    searchValue?: string
  }) {
    const url = 'Post/pending'
    return axiosClient.get<unknown, PendingPost[]>(url,{
      params:{
        categoryID,
        tagID,
        currentUserId,
        searchValue
      }
    })
  },
  postApproved() {
    const url = 'Post/all'
    return axiosClient.get<unknown, PendingPost[]>(url)
  },

  postCategoryTag({
    categoryID,
    tagID,
    currentUserId,
    searchValue
  }: {
    categoryID?: number[]
    tagID?: number[]
    currentUserId?: number
    searchValue?: string
  }) {
    const url = 'Post/on-load'
    return axiosClient.get<unknown, PendingPost[]>(url, {
      params: {
        categoryID,
        tagID,
        currentUserId,
        searchValue
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
  getUserPendingPost(userId:number){
    const url = `Post/pending/${userId}`
    return axiosClient.get<unknown, PendingPost[]>(url)
  },
  getUserMajorbyID(userID: number) {
    const url = `User/${userID}/majors`
    return axiosClient.get<unknown, UserMajor[]>(url)
  },
  getUserSubjectbyID(userID: number) {
    const url = `User/${userID}/subjects`
    return axiosClient.get<unknown, UserSubject[]>(url)
  },

  //category
  getAllCategory() {
    const url = 'Major/all'
    return axiosClient.get<unknown, Category[]>(url)
  },

  deletePost(postId: number) {
    const url = `Post/${postId}`
    return axiosClient.delete(url)
  },

  //tag
  getAllTag() {
    const url = 'Subject/all'
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
  getUserById(userID: number) {
    const url = `User/${userID}`
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

  followerByUserId(currentUserID: number, userID: number) {
    const url = `User/${userID}/follower`
    return axiosClient.get<unknown, User[]>(url, {
      params: {
        currentUserID
      }
    })
  },

  followingByUserId(currentUserID: number, userID: number) {
    const url = `User/${currentUserID}/following`
    return axiosClient.get<unknown, User[]>(url, {
      params: {
        userID
      }
    })
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
  createdUserMajor({ userID, majorID }: { userID: number; majorID: number[] }) {
    const url = `User/${userID}/major`
    return axiosClient.post(url, null, {
      params: {
        userID,
        majorID
      }
    })
  },

  deleteUserMajor(userID: number, majorID: number[]) {
    const url = `User/${userID}/major`
    return axiosClient.delete(url, {
      params: {
        majorID
      }
    })
  },
  createdUserSubject({ userID, subjectID }: { userID: number; subjectID: number[] }) {
    const url = `User/${userID}/subject`
    return axiosClient.post(url, null, {
      params: {
        userID,
        subjectID
      }
    })
  },
  deleteUserSubject(userID: number, subjectID: number[]) {
    const url = `User/${userID}/subject`
    return axiosClient.delete(url, {
      params: {
        subjectID
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
    return axiosClient.get<unknown, Post>(url, {
      params: {
        currentUserId: localStorage.getItem('id')
      }
    })
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

  votePost({ currentUserId, postId, vote }: { currentUserId: number; postId: number; vote: number }) {
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

  voteUpdate({ currentUserId, postId, vote }: { currentUserId: number; postId: number; vote: number }) {
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

  allPostHasImages(currentUserId: number) {
    const url = 'Post/all-post-has-image'
    return axiosClient.get<unknown, PendingPost[]>(url, { params: { currentUserId } })
  },

  allPostHasVideo(currentUserId: number) {
    const url = 'Post/all-post-has-video'
    return axiosClient.get<unknown, PendingPost[]>(url, { params: { currentUserId } })
  },

  deleteVote(postId: number, currentUserId: number) {
    const url = 'VotePost'
    return axiosClient.delete(url, {
      params: {
        currentUserId,
        postId
      }
    })
  },

  follow(currentUserID: number, userID: number) {
    const url = 'User/follow'
    return axiosClient.post(url, null, {
      params: {
        currentUserID,
        userID
      }
    })
  },

  unFollow(currentUserID: number, userID: number) {
    const url = 'User/follow'
    return axiosClient.delete(url, {
      params: {
        currentUserID,
        userID
      }
    })
  },

  trendingTag() {
    const url = 'Subject/top-5-voted'
    return axiosClient.get<unknown, Tag[]>(url)
  },

  trendingPost() {
    const url = 'Post/top-5-voted'
    return axiosClient.get<unknown, Post[]>(url)
  },

  trendingCategory() {
    const url = 'Major/top-5-voted'
    return axiosClient.get<unknown, Category[]>(url)
  }
}

export default api
