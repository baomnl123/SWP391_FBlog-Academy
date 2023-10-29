import React from "react";
import { useParams } from "react-router-dom";
import ListUsers from "../List/ListUser";
import ReactPlayer from "react-player";
import { Col, Container, Card, Row, CardTitle, Icon } from "react-materialize";

export default function UserDetail() {
  const nameUser = useParams();
  const user = ListUsers.find((user) => {
    return user.id == nameUser.id;
  });

  // const { title } = useParams();

  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <Container>
        <Row>
          <Col m={4}>
            <div className="user-image">
              <img src={user.image} alt="" width={"100%"} />
            </div>
          </Col>
          <Col m={8}>
            <h4 style={{ fontWeight: "700" }}>{user.title}</h4>
            <p style={{ textAlign: "justify" }}></p>
            <p>
              <span style={{ fontWeight: "700", fontSize: "18px" }}>Role</span>
              :&nbsp;
              {user.role}
            </p>
          </Col>
        </Row>
      </Container>
    </div>
  );
}
