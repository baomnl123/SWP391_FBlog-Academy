import React from "react";
import { useRef, useState, useEffect } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Modal from "react-bootstrap/Modal";
import Comments from "../comments/Comments";
function CmtBtn() {
  const [show, setShow] = useState(false);
  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  return (
    <>
      <Button className="Comment-Btn" onClick={handleShow}>
        Comment
      </Button>
      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Modal heading</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Comments />
        </Modal.Body>
      </Modal>
    </>
  );
}

export default CmtBtn;
