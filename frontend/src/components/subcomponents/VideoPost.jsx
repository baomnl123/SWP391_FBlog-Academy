import { useRef, useState, useEffect } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Modal from "react-bootstrap/Modal";
import DeleteButton from "../DeleteButton";
export default function VideoPost({
  dp,
  name,
  ago,
  likes,
  comments,
  shares,
  title,
  desc,
  post,
  reactType,
  border,
  html,
}) {
  const [isLike, setIsLike] = useState(false);
  const [isMenu, setIsMenu] = useState(false);
  const [isDelete, setDelete] = useState(false);

  const likeBtn = useRef();
  const reactQuantity = useRef();
  const dotMenu = useRef();
  const container = useRef();
  const video = useRef();

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
      video.current.pause();
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

  return (
    <div className="newsfeed flex-column" ref={container}>
      <div className="news-header flex-row">
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
        <div className="dot-menu hover2" ref={dotMenu} onClick={handleMenu}>
          <img src="images/dotmenu.png" alt="" className="news-icon dot" />
          <div className="triangle"></div>
          <small className="del-post" onClick={handleDelete}>
            <DeleteButton />
          </small>
        </div>
      </div>
      <div className="paragraph">
        {html ? (
          <p dangerouslySetInnerHTML={{ __html: title }}></p>
        ) : newPost ? (
          <p className="post-par">{title}</p>
        ) : (
          <p>{title}</p>
        )}
        {html ? (
          <p dangerouslySetInnerHTML={{ __html: desc }}></p>
        ) : (
          <p>{desc}</p>
        )}
      </div>
      <div className="post attention">
        <video autoPlay controls muted ref={video}>
          <source src={post} />
        </video>
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
            <p>{shares} Shares</p>
          </div>
        </div>
        <div className="flex-row interact mind share">
          <div
            className="flex-row interact-icons like-container"
            id="com-react"
            ref={likeBtn}
            onClick={handleLike}
          >
            <div className="like-icon"></div>
            <p className="like">Like</p>
          </div>
          <div className="flex-row interact-icons comment" id="com-react">
            <img src="images/comment.png" alt="" />
            <Button className="Comment-Btn" onClick={handleShow}>
              Comment
            </Button>
            <Modal show={show} onHide={handleClose}>
              <Modal.Header closeButton>
                <Modal.Title>Modal heading</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                <Form>
                  <Form.Group
                    className="mb-3"
                    controlId="exampleForm.ControlTextarea1"
                  >
                    <Form.Label>Hãy viết suy nghĩ của bạn lên blog</Form.Label>
                    <Form.Control as="textarea" rows={3} />
                  </Form.Group>
                </Form>
              </Modal.Body>
              <Modal.Footer>
                <Button variant="primary" onClick={handleClose}>
                  Comment
                </Button>
              </Modal.Footer>
            </Modal>
          </div>
          <div className="flex-row interact-icons" id="com-react">
            <img src="images/report.png" alt="" />
            <p>Report</p>
          </div>
          <div className="flex-row interact-icons" id="com-react">
            <DeleteButton />
          </div>
        </div>
      </div>
    </div>
  );
}
