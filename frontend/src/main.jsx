import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import {
  ClerkProvider,
  RedirectToSignIn,
  SignIn,
  SignUp,
  SignedIn,
  SignedOut,
  useUser,
} from "@clerk/clerk-react";
import { BrowserRouter, Routes, Route, useNavigate } from "react-router-dom";
import HomePage from "./components/Page/HomePage.jsx";
import { ChakraProvider } from "@chakra-ui/react";
if (!import.meta.env.VITE_CLERK_PUBLISHABLE_KEY) {
  throw new Error("Missing Publishable Key");
}
const clerkPubKey = import.meta.env.VITE_CLERK_PUBLISHABLE_KEY;
console.log(clerkPubKey);

const ClerkWithRoutes = () => {
  const navigate = useNavigate();
  // const { isSignedIn, user, isLoaded } = useUser();

  // if (!isLoaded) {
  //   return null;
  // }

  // if (isSignedIn) {
  //   const redirectUrl = "/protected";
  // }

  return (
    <ClerkProvider publishableKey={clerkPubKey} navigate={(to) => navigate(to)}>
      <Routes>
        <Route path="*" element={<App />} />
        <Route
          path="/sign-in/*"
          element={<SignIn redirectUrl={"/"} routing="path" path="/sign-in" />}
        />
        <Route
          path="/sign-up/*"
          element={<SignUp redirectUrl={"/"} routing="path" path="/sign-up" />}
        />
        <Route
          path="/HomePage"
          element={
            <>
              <SignedIn>
                <App />
              </SignedIn>
              <SignedOut>
                <RedirectToSignIn />
              </SignedOut>
            </>
          }
        />
      </Routes>
    </ClerkProvider>
  );
};

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <BrowserRouter>
      <ChakraProvider>
        <ClerkWithRoutes />
      </ChakraProvider>
    </BrowserRouter>
  </React.StrictMode>
);
