import './App.scss';
import React from 'react';

function NavigationBar() {
  return (
    <div className="NavigationBar">
      <h1>Status</h1>
      <div className="NavigationBar-ButtonWrapper">
        <button>=</button>
      </div>
    </div>
  );
}

function PingGraph() {
  return (
    <div></div>
  );
}

function Incidents() {
  return (
    <div className="Incidents">
      <h2 className="Incidents-Header">Latest Incident</h2>
      <h4 className="Incidents-Title">Expected maintenance schedule for 2023-01-31 04:30:00 GMT-7</h4>
      <p className="Incidents-Body">
          The following services will be down on January 31st between 4:30 AM PST and 6:30 AM PST while we perform regular maintenance:<br/>
          - Authentication<br/>
          - API<br/>
          - Git<br/>
      </p>
      <svg className="Incidents-DropdownButton" width="2em" height="2em" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
      <rect width="48" height="48" fill="white" fill-opacity="0.01"/>
      <path d="M40 28L24 40L8 28" stroke="#747c92" stroke-width="4" stroke-linecap="round" stroke-linejoin="round"/>
      <path d="M8 10H40" stroke="#747c92" stroke-width="4" stroke-linecap="round"/>
      <path d="M8 18H40" stroke="#747c92" stroke-width="4" stroke-linecap="round"/>
      </svg>
    </div>
  );
}

function App() {
  return (
    <div className="App">
      <NavigationBar />
      <PingGraph />
      <div className="Incidents-Container"><Incidents /></div>
    </div>
  );
}

export default App;