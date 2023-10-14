import { useState } from "react";
import PhotoPost from "./subcomponents/PhotoPost";
import VideoPost from "./subcomponents/VideoPost";

export default function Newsfeed() {
    const kodeGo = "Start your tech career with KodeGo!<br></br>Take it from John Michael Baldonado, a KodeGo graduate who landed his dream job in the tech industry even without a college degree.<br></br>Here's what John Michael liked most about our bootcamp:<br></br>✅ Teamwork when accomplishing tasks<br>✅ Hands-on training & beginner-friendly lessons<br>✅ Endorsement to various companies after the bootcamp<br></br>Pursue your dream career. Sign up at KodeGo today."
    const newJeans = "<i className='fa-solid fa-headphones'></i> 'Attention' - NewJeans"
    const portfolio = "@robindc | Portfolio<br></br>🚀 Techstacks I use recently : HTML, CSS, JavaScript, React, Vite, Tailwind, Materialize, Bootstrap, Mocha and Chai, Postman, Npm.<br></br>I create stuffs for fun, transforming your ideas into reality.<br></br>Link : <a className='portfolio' href='https://robindc.vercel.app/' target='_blank'>https://robindc.vercel.app/</a>"

    const [newPost, setNewPost] = useState([])

    function setPost(post) {
        setNewPost(prevPost => [...prevPost, post])
    }
    return (
        <main className="flex-column main">




            <PhotoPost dp={"images/robin.png"} name={"Khuong Duy"} ago={"1d"} desc={"Lịch Fbus mới cho ae "} post={"images/fbus.jpg"} likes={56} comments={87} shares={1} reactType={"images/react2.png"} />

            <VideoPost dp={"images/anymous.jpg"} name={"Hoàng Thanh"} ago={"Sep 29"} desc={"Mọi người cho em hỏi cách fix sập API ạ :<< !! Video chống trôi"} post={"images/attention.mp4"} likes={"3K"} comments={45} shares={30} reactType={"images/react2.png"} border={false} html={true} />

            <PhotoPost dp={"images/anymous.jpg"} name={"Hòa Hiếu"} ago={"35m"} desc={"Tìm đồng đội SWP391 ạ !! nhóm em hiện tại đang thiếu 2 người ạ"} post={"images/longbao.jpg"} likes={"4.6K"} comments={5} shares={7} reactType={"images/react.png"} />

            <PhotoPost dp={"images/robin.png"} name={"Khương Duy"} ago={"1d"} desc={"Hôm qua em nghe bảo kì này trường đổi ngân hàng đề ạ :(("} post={""} likes={567} comments={39} shares={5} reactType={"images/react2.png"} border={false} html={true} />

            <PhotoPost dp={"images/anymous.jpg"} name={"Thanh Phương"} ago={"1h"} desc={"Cho em hỏi mình có nên hoãn Trans để học Ielts bên ngoài k ạ !"} post={"https://i.pinimg.com/originals/dd/2f/5d/dd2f5dcfc74590aa113bbbaed6ee8a57.gif"} likes={"1.1K"} comments={9} shares={8} reactType={"images/react.png"} border={true} />
        </main>

    )
}
