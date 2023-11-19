import { Navigate, createBrowserRouter } from 'react-router-dom'

import PrivateRoute from './PrivateRoute'

import Dashboard from '@/pages/dashboard'
import Lecture from '@/pages/lecture'
import Major from '@/pages/major'
import Profile from '@/pages/profile'
import Student from '@/pages/student'
import Subject from '@/pages/subject'

import ReportPost from '@/pages/report'
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
        path: '/major',
        element: <Major />,
        handle: {
          crumb: () => 'Major List'
        }
      },
      {
        path: '/subject',
        element: <Subject />,
        handle: {
          crumb: () => 'Subject List'
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
        path: '/report',
        element: <ReportPost />,
        handle: {
          crumb: () => 'Report'
        }
      },
      {
        path: '/policies',
        element: <Profile />,
        handle: {
          crumb: () => 'Policies'
        }
      },

      {
        path: '*',
        element: <Navigate to='/' />
      }
    ]
  }
])
