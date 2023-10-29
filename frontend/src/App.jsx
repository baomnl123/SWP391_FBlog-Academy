import Loader from "./components/Loader";
import { useState } from "react";
import Topbar from "./components/Topbar";
import Leftbar from "./components/Leftbar";
import PostBlog from "./components/Rightbar";
import Router from "./routers/route";
import UserDetail from "./components/UserDetail";
import { useUser } from "@clerk/clerk-react";
import "bootstrap/dist/css/bootstrap.min.css";
import Newsfeed from "./components/Newsfeed";
import TopbarLogin from "./components/Authenticated User/TopbarLogin";

function App() {
  const [isLoading, setIsLoading] = useState(true);
  const { isSignedIn, user, isLoaded } = useUser();
  const handleLoaderFinished = () => {
    setIsLoading(false);
  };

  if (isSignedIn) {
    return (
      <>
        {isLoading ? (
          <Loader onLoaderFinished={handleLoaderFinished} />
        ) : (
          <>
            <TopbarLogin />
            <Leftbar />
            <Router />
          </>
        )}
      </>
    );
  }

  return (
    <>
      {isLoading ? (
        <Loader onLoaderFinished={handleLoaderFinished} />
      ) : (
        <>
          <Topbar />
          <Router />
        </>
      )}
    </>
  );
}

export default App;
