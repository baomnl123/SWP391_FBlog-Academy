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
