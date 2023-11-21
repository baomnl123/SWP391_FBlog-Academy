export type Image = {
  id: number
  postId: number
  url: string
  createdAt: Date
  status: boolean
}

export type Post = {
  id: number
  userId: number
  reviewerId: number
  title: string
  content: string
  videos: Image[]
  images: Image[]
  majors: Major[] | null
  subjects: Subject[] | null
  createdAt: Date
  updatedAt: Date
  isApproved: boolean
  status: boolean
  user?: User
}

export type PendingPost = {
  id: number
  user: User
  reviewerId: null
  title: string
  content: string
  videos: Image[]
  images: Image[]
  majors: null
  subjects: null
  createdAt: Date
  updatedAt: null
  isApproved: boolean
  status: boolean
  upvotes?: number
  upvote?: number
  downvote?: number
}

export type PostByUserId = {
  id: number
  user: User
  reviewerId: number
  title: string
  content: string
  videos: Image[]
  images: Image[]
  majors: null
  subjects: null
  createdAt: Date
  updatedAt: Date
  isApproved: boolean
  status: boolean
}

export type User = {
  id: number
  name: string
  email: string
  avatarUrl: string
  role: string
  followerNumber: number
  postNumber: number
  createdAt: Date
  updatedAt: Date
  status: boolean
  isAwarded: boolean
  isFollowed: boolean
}

export type Major = {
  id: number
  adminId: number
  majorName: string
  createdAt: Date
  updatedAt: Date
  status: boolean
}

export type Subject = {
  id: number
  adminId: number
  subjectName: string
  createdAt: Date
  updatedAt: null
  status: boolean
}

export type SavePost = {
  id?: number
  userId?: number
  name?: string
  createdAt?: Date
  updateAt?: null
  status?: boolean
}
