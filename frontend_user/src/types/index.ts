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
  categories: Category[] | null
  tags: Tag[] | null
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
  categories: null
  tags: null
  createdAt: Date
  updatedAt: null
  isApproved: boolean
  status: boolean
  upvotes?: number
  upvote?: boolean
  downvote?: boolean
}

export type PostByUserId = {
  id: number
  user: User
  reviewerId: number
  title: string
  content: string
  videos: Image[]
  images: Image[]
  categories: null
  tags: null
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
  createdAt: Date
  updatedAt: Date
  status: boolean
  isAwarded: boolean
  isFollowed: boolean
}

export type Category = {
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
  tagName: string
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
