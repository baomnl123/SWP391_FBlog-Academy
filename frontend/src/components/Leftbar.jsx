//import Groups from "./subcomponents/Groups";

import { Routes, Route } from "react-router-dom";
import { Popup } from "reactjs-popup";
import { useState } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Modal from "react-bootstrap/Modal";
export default function Leftbar() {
  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  return (
    <nav className="left-bar">
      <div className="signal"></div>

      <div className="flex-item">
        <div>
          <a href="/" className="side-icon hover2">
            <i className="fa-solid fa-house"></i>
            <font>Home</font>
          </a>
        </div>
      </div>

      <div className="flex-item">
        <div>
          <a href="/" className="side-icon2 hover2">
            <div className="circle-container">
              <img src="images/group.png" alt="" />
            </div>
            <font>Follwing</font>
          </a>
        </div>
        <div>
          <a href="/VideoPost" className="side-icon2 hover2">
            <div className="circle-container2">
              <img src="images/watch.png" alt="" />
            </div>
            <font>Blog have Video</font>
          </a>
        </div>

        <div>
          <a href="/PhotoPost" className="side-icon2 hover2">
            <div className="circle-container2">
              <img src="images/gaming.png" alt="" />
            </div>
            <font>Blog have picture</font>
          </a>
        </div>
      </div>

      <div className="flex-item">
        <div>
          <a href="#!" className="side-icon2 hover2">
            <div className="circle-container">
              <img src="images/link.png" alt="" />
            </div>
            <font>Shortcuts</font>
          </a>
        </div>

        <Button onClick={handleShow}>Post Blog</Button>

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
              Đăng Blog
            </Button>
          </Modal.Footer>
        </Modal>
      </div>
    </nav>
  );
}
