export interface Post {
  id: number
  userId?: number
  reviewerId?: number
  title?: string
  content?: string
  videos?: Video[]
  images?: Image[]
  majors?: string
  subjects?: Tag[]
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
  subjectName?: string
  createdAt?: string
  updatedAt?: string
  status?: boolean
}

export interface CreatePostBodyRequest {
  title: string
  content: string
  subjectIds: number[]
  majorIds: number[]
  videoURLs: string[]
  imageURLs: string[]
}
