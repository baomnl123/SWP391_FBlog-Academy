import api from './axiosClient'

export const getAllPost = () => api.get('/Post/all')
