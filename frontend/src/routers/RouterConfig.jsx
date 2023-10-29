import { Navigate, createBrowserRouter } from "react-router-dom";

import HomePage from "../components/Page/HomePage";
import PhotoPostPage from "../components/PhotoPostPage";
import VideoPostPage from "../components/VideoPostPage";
import ShowFollwing from "../components/ShowFollowing";
import { RedirectToSignIn, SignedIn, SignedOut } from "@clerk/clerk-react";
export const Routes = createBrowserRouter([
  {
    path: "/",
    element: (
      <>
        <SignedIn>
          <HomePage />
        </SignedIn>
        <SignedOut>
          <RedirectToSignIn />
        </SignedOut>
      </>
    ),
    children: [
      {
        index: true,
        element: <Navigate to="/HomePage" />,
      },
      {
        path: "/HomePage",
        element: <HomePage />,
        handle: {
          crumb: () => "Home",
        },
      },
      {
        path: "/PhotoPost",
        element: <PhotoPostPage />,
        handle: {
          crumb: () => "Category",
        },
      },
      {
        path: "/VideoPost",
        element: <VideoPostPage />,
        handle: {
          crumb: () => "Tag",
        },
      },

      {
        path: "*",
        element: <Navigate to="/" />,
      },
    ],
  },
]);
