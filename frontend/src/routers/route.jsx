import { Routes, Route, Navigate } from "react-router-dom";
import Newsfeed from "../components/Newsfeed";
import VideoPost from "../components/VideoPostPage";
import PhotoPost from "../components/PhotoPostPage";
import PostBlog from "../components/PostBlog";
import HomePage from "../components/Page/HomePage";
import ShowFollwing from "../components/ShowFollowing";

const Router = () => {
  return (
    <Routes>
      <Route path="*" element={<Newsfeed />} />
      <Route path="/VideoPost" element={<VideoPost />} />
      <Route path="/PhotoPost" element={<PhotoPost />} />
      <Route path="/PostBlog" element={<PostBlog />} />
      <Route path="/Following" element={<ShowFollwing />} />
    </Routes>
  );
};
export default Router;
