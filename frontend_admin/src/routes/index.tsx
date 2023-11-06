import { RouterProvider } from 'react-router-dom'
import { Routes } from './RouterConfig'

export default function AppRoute() {
  return <RouterProvider router={Routes} />
}
