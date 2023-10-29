import { useState } from "react";
import VideoPost from "./subcomponents/VideoPost";
import { UserButton } from "@clerk/clerk-react";
const VideoPostPage = () => {
  return (
    <>
      <main className="flex-column main">
        <VideoPost
          dp={"images/anymous.jpg"}
          name={"Hoàng Thanh"}
          ago={"Sep 29"}
          desc={
            "Mọi người cho em hỏi cách fix sập API ạ :<< !! Video chống trôi"
          }
          post={"images/attention.mp4"}
          likes={"3K"}
          comments={45}
          shares={30}
          reactType={"images/react4.png"}
          border={false}
          html={true}
        />
      </main>
    </>
  );
};
export default VideoPostPage;
