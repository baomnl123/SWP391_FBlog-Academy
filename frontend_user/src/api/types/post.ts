export interface Post {
  id: number
  userId?: number
  reviewerId?: number
  title?: string
  content?: string
  videos?: Video[]
  images?: Image[]
  categories?: string
  tags?: Tag[]
  createdAt?: string
  updatedAt?: string
  isApproved?: boolean
  status?: boolean
}

export interface Image {
  id?: number
  postId?: number
  url?: string
  createdAt?: string
  status?: boolean
}

export interface Video {
  id?: number
  postId?: number
  url?: string
  createdAt?: string
  status?: boolean
}

export interface Tag {
  id?: number
  adminId?: number
  tagName?: string
  createdAt?: string
  updatedAt?: string
  status?: boolean
}

export interface CreatePostBodyRequest {
  title: string
  content: string
  tagIds: number[]
  categoryIds: number[]
  videoURLs: string[]
  imageURLs: string[]
}
