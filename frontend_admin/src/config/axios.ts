// api/axiosClient.js
import { notification } from 'antd'
import axios from 'axios'
import queryString from 'query-string'

const axiosClient = axios.create({
  baseURL: 'https://fblogapi.azurewebsites.net/api/',
  headers: {
    'content-type': 'multipart/form-data'
  },
  paramsSerializer: (params) => queryString.stringify(params),
  timeout: 10 * 1000
})

axiosClient.interceptors.request.use(async (config) => {
  return config
})

axiosClient.interceptors.response.use(
  (response) => {
    if (response && response.data) {
      return response.data
    }
    return response.data
  },
  (error) => {
    notification.error({
      message: 'Error',
      description: error.message
    })
    throw error
  }
)
export default axiosClient
