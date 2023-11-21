export type Major = {
  id: number
  majorName: string
  createdAt: Date
  updatedAt: Date
  status: boolean
}

export type Subject = {
  id: number
  major: Major[]
  subjectName: string
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
  successReportedTimes: number
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
export type Media = {
  id: number
  postId: number
  type: string
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
  videos: Media[]
  images: Media[]
  categories: null
  subjects: null
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
  downvotes: number
  videos: Media[]
  images: Media[]
  majors: Major[]
  subjects: Subject[]
  createdAt: Date
  updatedAt: Date
  isApproved: boolean
  status: boolean
  reports: number
  reportList: {
    admin: unknown
    content: string
    createdAt: string
    post: Post
    reporter: User
    status: string
  }[]
}
