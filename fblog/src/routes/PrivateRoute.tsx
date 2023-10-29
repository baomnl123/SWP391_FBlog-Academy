import { Outlet } from 'react-router-dom'
import BaseLayout from '@/components/BaseLayout'

export default function PrivateRoute() {
  return (
    <BaseLayout>
      <Outlet />
    </BaseLayout>
  )
}
