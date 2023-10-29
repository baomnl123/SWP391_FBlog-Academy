import Loader from "../Loader";
import { useState } from "react";
import TopbarLogin from "../Authenticated User/TopbarLogin";
import Leftbar from "../Leftbar";

import Router from "../../routers/route";

import { useEffect } from "react";
import { gapi } from "gapi-script";

import "bootstrap/dist/css/bootstrap.min.css";

function HomePage() {
  const [isLoading, setIsLoading] = useState(true);

  const handleLoaderFinished = () => {
    setIsLoading(false);
  };

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

export default HomePage;
