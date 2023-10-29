import { Col, Container, Card, Row, CardTitle, Icon } from "react-materialize";
import { Link } from "react-router-dom/dist";
import { useState, useEffect, useContext } from "react";

export default function Follwing({ user }) {
  return (
    <main className="flex-column main">
      <div className="field" style={{ minHeight: "100vh" }}>
        <Container>
          <Row>
            {user.map((user) => (
              <Col m={6} s={12}>
                <Card
                  header={<CardTitle image={user.image} />}
                  title={user.title}
                  style={{
                    overflow: "hidden",
                  }}
                >
                  <Link to={`detail/${user.id}`}>
                    <a href="">View Profile</a>
                  </Link>
                </Card>
              </Col>
            ))}
          </Row>
        </Container>
      </div>
    </main>
  );
}
