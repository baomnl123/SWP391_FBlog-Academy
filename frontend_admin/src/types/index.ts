export type Categories = {
  id: number
  adminId: number
  categoryName: string
  createdAt: Date
  updatedAt: Date
  status: boolean
}

export type Tag = {
  id: number
  adminId: number
  categories: {
    id: number
    adminId: number
    categoryName: string
    createdAt: Date
    updatedAt: Date
    status: boolean
  }[]
  tagName: string
  createdAt: Date
  updatedAt: Date
  status: boolean
}

export type UserEmail = {
  id: number
  name: string
  email: string
  avatarUrl: string
  role: string
  createdAt: Date
  updatedAt: null
  status: boolean
  isAwarded: boolean
}

export interface Lecturers {
  id: number
  name: string
  email: string
  avatarUrl: string
  role: string
  createdAt: Date
  updatedAt: Date
  status: boolean
  isAwarded: boolean
}
export type Image = {
  id: number
  postId: number
  url: string
  createdAt: Date
  status: boolean
}

export type PendingPost = {
  id: number
  user: User
  reviewerId: null
  title: string
  content: string
  videos: Image[]
  images: Image[]
  categories: null
  tags: null
  createdAt: Date
  updatedAt: null
  isApproved: boolean
  status: boolean
}

export type User = {
  id: number
  name: string
  email: string
  avatarUrl: string
  role: string
  createdAt: Date
  updatedAt: Date
  status: boolean
  isAwarded: boolean
}

export type ReportPost = {
  reporter: User
  post: Post
  admin: User
  content: string
  status: string
  createdAt: Date
}

export interface Post {
  id: number
  user: User
  reviewerId: number
  title: string
  content: string
  upvotes: number
  videos: Image[]
  images: Image[]
  categories: { id: number; adminId: number; categoryName: string; createdAt: Date; updatedAt: Date; status: boolean }[]
  tags: { id: number; adminId: number; categoryName: string; createdAt: Date; updatedAt: Date; status: boolean }[]
  createdAt: Date
  updatedAt: Date
  isApproved: boolean
  status: boolean
}
