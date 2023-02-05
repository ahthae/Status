import './App.scss';
import React from 'react';
import LatestIncident from './LatestIncident';
import NavigationBar from './NavigationBar';

function PingGraph() {
  return (
    <div></div>
  );
}

function App() {
  return (
    <div className="App">
      <NavigationBar />
      <PingGraph />
      <div className="LatestIncident-Container"><LatestIncident /></div>
    </div>
  );
}

export default App;