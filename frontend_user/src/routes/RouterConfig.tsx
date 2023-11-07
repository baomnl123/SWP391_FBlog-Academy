import { Navigate, createBrowserRouter } from 'react-router-dom'
import PrivateRoute from './PrivateRoute'
import { RedirectToSignIn, SignedIn, SignedOut } from '@clerk/clerk-react'
import Dashboard from '@/pages/Dashboard'
import PendingList from '@/pages/PendingList'
import GiveAwards from '@/pages/GiveAwards'
import UserProfile from '@/pages/UserProfile'
import SaveList from '@/pages/SaveList'
import Promote from '@/pages/Promote'
import SavedPost from '@/pages/SaveList/SavedPost'

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
          crumb: () => ''
        }
      },
      {
        path: '/view-pending-list',
        element: <PendingList />,
        handle: {
          crumb: () => 'View Pending List'
        }
      },
      {
        path: '/give-awards',
        element: <GiveAwards />,
        handle: {
          crumb: () => 'Give Awards'
        }
      },
      {
        path: '/profile/:id',
        element: <UserProfile />
      },
      {
        path: '/save-list',
        element: <SaveList />,
        handle: {
          crumb: () => 'Save List'
        }
      },
      {
        path: '/save-list/:id',
        element: <SavedPost />,
        handle: {
          crumb: () => 'Saved Post'
        }
      },
      {
        path: '/promote',
        element: <Promote />,
        handle: {
          crumb: () => 'Promote'
        }
      },
      {
        path: '*',
        element: <Navigate to='/' />
      }
    ]
  }
])
