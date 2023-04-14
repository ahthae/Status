import './App.scss';
import React from 'react';
import LatestIncident from './components/LatestIncident';
import NavigationBar from './components/NavigationBar';
import StatusGraphs from './components/StatusGraphs';

const testServers = [
  {
    "id": "6433b690277522ff0a4f2839",
    "url": "https://jira.ahthae.net",
    "responses": [
      {
        "responseTime": "00:00:00.7474462",
        "timestamp": "2023-04-10T07:28:51.913Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:00.6643517",
        "timestamp": "2023-04-10T07:29:01.899Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:01.2248730",
        "timestamp": "2023-04-10T07:44:26.641Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:00.7460791",
        "timestamp": "2023-04-10T07:44:36.219Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:02.9579110",
        "timestamp": "2023-04-13T22:53:18.036Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      }
    ]
  },
  {
    "id": "6433b690277522ff0a4f283a",
    "url": "https://git.ahthae.net",
    "responses": [
      {
        "responseTime": "00:00:01.2880862",
        "timestamp": "2023-04-10T07:28:51.913Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:00.9962657",
        "timestamp": "2023-04-10T07:29:01.899Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:01.1442516",
        "timestamp": "2023-04-10T07:44:26.641Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:00.7487875",
        "timestamp": "2023-04-10T07:44:36.219Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      },
      {
        "responseTime": "00:00:02.7476374",
        "timestamp": "2023-04-13T22:53:18.036Z",
        "statusCode": 200,
        "reasonPhrase": "OK"
      }
    ]
  }
]

function App() {

  return (
    <div className="App">
      <NavigationBar />
      <StatusGraphs servers={testServers} />
      <LatestIncident />
    </div>
  );
}

export default App;