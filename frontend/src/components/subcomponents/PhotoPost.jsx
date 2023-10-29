import { Text } from "@chakra-ui/react";
import { useRef, useState, useEffect } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Modal from "react-bootstrap/Modal";
import DeleteButton from "../DeleteButton";
import CmtBtn from "../CmtBtn/CmtBtn";
export default function PhotoPost({
  dp,
  name,
  ago,
  likes,
  comments,
  Report,
  title,
  desc,
  post,
  reactType,
  border,
  html,
  key,
  newPost,
}) {
  const [isLike, setIsLike] = useState(false);
  const [isMenu, setIsMenu] = useState(false);
  const [isDelete, setDelete] = useState(false);

  const likeBtn = useRef();
  const reactQuantity = useRef();
  const dotMenu = useRef();
  const container = useRef();

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  useEffect(() => {
    if (isLike) {
      likeBtn.current.classList.toggle("active");
      reactQuantity.current.classList.toggle("activequan");
    }
  }, [isLike]);

  useEffect(() => {
    if (isMenu) {
      dotMenu.current.classList.toggle("active");
    }
  }, [isMenu]);

  useEffect(() => {
    if (isDelete) {
      container.current.remove();
    }
  }, [isDelete]);

  useEffect(() => {
    const handleScroll = () => {
      if (dotMenu.current.classList.contains("active")) {
        dotMenu.current.classList.remove("active");
      }
    };
    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  function handleLike() {
    setIsLike((like) => !like);
  }
  function handleMenu() {
    setIsMenu((menu) => !menu);
  }
  function handleDelete() {
    setDelete((del) => !del);
  }

  const style = {
    width: newPost ? "100%" : "",
  };
  return (
    <>
      <div
        className="newsfeed flex-column"
        ref={container}
        key={key}
        style={style}
      >
        <div className="news-header flex-row paragraph-box">
          <div className="flex-row">
            <div className="news-dp">
              <img src={dp} alt="" />
            </div>
            <div className="dp-name">
              <p>
                <strong>{name}</strong>
              </p>
              <div className="flex-row under-name">
                <small>{ago} •</small>
                <img src="images/globe.png" alt="" className="news-icon" />
              </div>
            </div>
          </div>
        </div>
        <div className="paragraph">
          {/* Get data */}
          {/* Import data to Newsfeed */}
          {html ? (
            <p dangerouslySetInnerHTML={{ __html: title }}></p>
          ) : newPost ? (
            <h1 className="title">{title}</h1>
          ) : (
            <h1>{title}</h1>
          )}
          {html ? (
            <p dangerouslySetInnerHTML={{ __html: desc }}></p>
          ) : newPost ? (
            <p className="post-par">{desc}</p>
          ) : (
            <p>{desc}</p>
          )}
        </div>
        <div className="post">
          {newPost ? (
            ""
          ) : border ? (
            <div className="pic-container aespa flex-row">
              <img src={post} alt="" />
            </div>
          ) : (
            <div className="pic-container">
              <img src={post} alt="" />
            </div>
          )}

          <div className="pic-comment"></div>
        </div>
        <div className="news-footer flex-column">
          <div className="flex-row likes">
            <div className="flex-row">
              <img src={reactType} alt="" />
              <p className="react-quan" ref={reactQuantity}>
                {likes}
              </p>
            </div>
            <div className="flex-row comments">
              <p>{comments} Comments</p>
              <p>{Report} Report</p>
            </div>
          </div>
          <div className="flex-row interact mind share">
            <div>
              <div
                className="flex-row interact-icons like-container"
                id="com-react"
                ref={likeBtn}
                onClick={handleLike}
              >
                <div className="like-icon"></div>
                <p className="like">Like</p>
              </div>
            </div>
            <div className="flex-row interact-icons comment" id="com-react">
              <img src="images/comment.png" alt="" />
              <CmtBtn />
            </div>
            <div className="flex-row interact-icons" id="com-react">
              <img src="images/report.png" alt="" />
              <p>Report</p>
            </div>
            <div className="flex-row interact-icons" id="com-react">
              <DeleteButton />
            </div>
          </div>{" "}
        </div>
      </div>
    </>
  );
}
