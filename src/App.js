import './App.scss';
import React from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

function Incidents() {
  return (
    <Container className="Incidents rounded">
      <Row>
        <Col><h3 className="Incidents-Header m-2">Latest Incident</h3></Col>
      </Row>
      <Row>
        <Col className="d-flex align-items-center">
          <h4 className="Incidents-Title m-2 text-center">Expected maintenance schedule for 2023-01-31 04:30:00 GMT-7</h4>
        </Col>
        <Col xs={8}><p className="Incidents-Body m-2">
            The following services will be down on January 31st between 4:30 AM PST and 6:30 AM PST while we perform regular maintenance:<br/>
            - Authentication<br/>
            - API<br/>
            - Git<br/>
        </p></Col>
      </Row>
      <Row>
        <svg className="mb-2" width="2em" height="2em" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
        <rect width="48" height="48" fill="white" fill-opacity="0.01"/>
        <path d="M40 28L24 40L8 28" stroke="#747c92" stroke-width="4" stroke-linecap="round" stroke-linejoin="round"/>
        <path d="M8 10H40" stroke="#747c92" stroke-width="4" stroke-linecap="round"/>
        <path d="M8 18H40" stroke="#747c92" stroke-width="4" stroke-linecap="round"/>
        </svg>
      </Row>
    </Container>
  );
}

function App() {
  return (
    <div className="App">
      <Incidents />
    </div>
  );
}

export default App;