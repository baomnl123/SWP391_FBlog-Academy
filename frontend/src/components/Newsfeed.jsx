import PhotoPost from "./subcomponents/PhotoPost";
import VideoPost from "./subcomponents/VideoPost";
import { useState } from "react";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
export default function Newsfeed() {
  const values = [true, "sm-down", "md-down", "lg-down", "xl-down", "xxl-down"];
  const [fullscreen, setFullscreen] = useState(true);
  const [show, setShow] = useState(false);
  return (
    <main className="flex-column main">
      <PhotoPost
        dp={"images/robin.png"}
        name={"Khuong Duy"}
        ago={"1d"}
        title={"Lịch"}
        desc={"Lịch Fbus mới cho ae "}
        post={"images/fbus.jpg"}
        likes={56}
        comments={87}
        shares={1}
        reactType={"images/react4.png"}
      />

      <VideoPost
        dp={"images/anymous.jpg"}
        name={"Hoàng Thanh"}
        ago={"Sep 29"}
        desc={"Mọi người cho em hỏi cách fix sập API ạ :<< !! Video chống trôi"}
        post={"images/attention.mp4"}
        likes={"3K"}
        comments={45}
        shares={30}
        reactType={"images/react4.png"}
        border={false}
        html={true}
      />

      <PhotoPost
        dp={"images/anymous.jpg"}
        name={"Hòa Hiếu"}
        ago={"35m"}
        desc={"Tìm đồng đội SWP391 ạ !! nhóm em hiện tại đang thiếu 2 người ạ"}
        post={"images/longbao.jpg"}
        likes={"4.6K"}
        comments={5}
        shares={7}
        reactType={"images/react4.png"}
      />

      <PhotoPost
        dp={"images/robin.png"}
        name={"Khương Duy"}
        ago={"1d"}
        desc={"Hôm qua em nghe bảo kì này trường đổi ngân hàng đề ạ :(("}
        post={""}
        likes={567}
        comments={39}
        shares={5}
        reactType={"images/react4.png"}
        border={false}
        html={true}
      />

      <PhotoPost
        dp={"images/anymous.jpg"}
        name={"Thanh Phương"}
        ago={"1h"}
        desc={"Cho em hỏi mình có nên hoãn Trans để học Ielts bên ngoài k ạ !"}
        post={
          "https://i.pinimg.com/originals/dd/2f/5d/dd2f5dcfc74590aa113bbbaed6ee8a57.gif"
        }
        likes={"1.1K"}
        comments={9}
        shares={8}
        reactType={"images/react4.png"}
        border={true}
      />
    </main>
  );
}
