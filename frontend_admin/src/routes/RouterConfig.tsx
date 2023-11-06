import { Navigate, createBrowserRouter } from 'react-router-dom'

import PrivateRoute from './PrivateRoute'

import Dashboard from '@/pages/dashboard'
import Category from '@/pages/category'
import Lecture from '@/pages/lecture'
import Post from '@/pages/post'
import Profile from '@/pages/profile'
import Student from '@/pages/student'
import Tag from '@/pages/tag'
import { RedirectToSignIn, SignedIn, SignedOut } from '@clerk/clerk-react'

export const Routes = createBrowserRouter([
  {
    path: '/',
    element: (
      <>
        <SignedIn>
          <PrivateRoute />
        </SignedIn>
        <SignedOut>
          <RedirectToSignIn />
        </SignedOut>
      </>
    ),
    children: [
      {
        index: true,
        element: <Navigate to='/dashboard' />
      },
      {
        path: '/dashboard',
        element: <Dashboard />,
        handle: {
          crumb: () => 'Dashboard'
        }
      },
      {
        path: '/category',
        element: <Category />,
        handle: {
          crumb: () => 'Category List'
        }
      },
      {
        path: '/tag',
        element: <Tag />,
        handle: {
          crumb: () => 'Tag List'
        }
      },
      {
        path: '/student',
        element: <Student />,
        handle: {
          crumb: () => 'Student'
        }
      },
      {
        path: '/lecturer',
        element: <Lecture />,
        handle: {
          crumb: () => 'Lecturer'
        }
      },
      {
        path: '/post',
        element: <Post />,
        handle: {
          crumb: () => 'Post'
        }
      },
      {
        path: '/profile',
        element: <Profile />,
        handle: {
          crumb: () => 'Profile'
        }
      },
      {
        path: '*',
        element: <Navigate to='/' />
      }
    ]
  }
])
