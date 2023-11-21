import BaseLayout from '@/components/BaseLayout'
import api from '@/config/api'
import { useClerk, useUser } from '@clerk/clerk-react'
import { useRequest } from 'ahooks'
import { Outlet, useNavigate } from 'react-router-dom'

export default function PrivateRoute() {
  const { user } = useUser()
  const { signOut } = useClerk()
  const navigate = useNavigate()
  useRequest(
    async () => {
      const response = await api.getEmail(user?.emailAddresses[0].emailAddress ?? '')
      localStorage.setItem('id', response.id.toString())

       if (response.role !== 'AD') {
         signOut(() => navigate('/'))
       }
      return response
    },
    {
      onError(e) {
         signOut(() => navigate('/'))
        console.error(e)
      }
    }
  )

  return (
    <BaseLayout>
      <Outlet />
    </BaseLayout>
  )
}
