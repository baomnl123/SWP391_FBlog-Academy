import api from '@/api'
import { setUser } from '@/store/reducers/user'
import { User } from '@/types'
import { useUser } from '@clerk/clerk-react'
import { useRequest } from 'ahooks'
import { useCallback, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Outlet } from 'react-router-dom'

export default function PrivateRoute() {
  const { user } = useUser()

  const dispatch = useDispatch()
  // const { data: userInfo, runAsync: getUser } = useRequest(api.getUserByEmail, {
  //   manual: true,
  //   onSuccess: (res) => {
  //     console.log(res)
  //     if (res) {
  //       dispatch(setUser(res as unknown as User))
  //     }
  //   },
  //   onError: (err) => {
  //     // message.error('User not found')
  //     console.log(err)
  //   }
  // })

  const registerAction = useCallback(async () => {
    const formData = new FormData()
    formData.append('name', user?.fullName ?? 'student')
    formData.append('avatarUrl', user?.imageUrl ?? '')
    formData.append('email', user?.emailAddresses?.[0].emailAddress ?? '')
    const response = await api.createNewAccount(formData)
    dispatch(setUser(response as unknown as User))
  }, [dispatch, user?.emailAddresses, user?.fullName, user?.imageUrl])

  const { run: register } = useRequest(
    async () => {
      const isExist = await api.getUserByEmail({ email: user?.emailAddresses?.[0].emailAddress ?? '' })
      dispatch(setUser(isExist as unknown as User))
    },
    {
      manual: true,
      onError(e) {
        registerAction()
        console.error(e)
      }
    }
  )
  useEffect(() => {
    register()
  }, [register])

  // useEffect(() => {
  //   if (!user) {
  //     return
  //   }
  //   getUser({
  //     email: user?.emailAddresses?.[0].emailAddress ?? ''
  //   })
  // }, [user, getUser])
  return <Outlet />
}
