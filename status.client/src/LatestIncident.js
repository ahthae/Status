import './LatestIncident.scss';

export default function LatestIncident() {
  return (
    <div className="LatestIncident">
      <h2 className="LatestIncident-Header">Latest Incident</h2>
      <h4 className="LatestIncident-Title">Expected maintenance schedule for 2023-01-31 04:30:00 GMT-7</h4>
      <p className="LatestIncident-Body">
          The following services will be down on January 31st between 4:30 AM PST and 6:30 AM PST while we perform regular maintenance:<br/>
          - Authentication<br/>
          - API<br/>
          - Git<br/>
      </p>
      <svg className="LatestIncident-DropdownButton" xmlns="http://www.w3.org/2000/svg" width="1.8em" height="1.8em" fill="currentColor" viewBox="0 0 16 16">
      <path fill-rule="evenodd" d="M1.553 6.776a.5.5 0 0 1 .67-.223L8 9.44l5.776-2.888a.5.5 0 1 1 .448.894l-6 3a.5.5 0 0 1-.448 0l-6-3a.5.5 0 0 1-.223-.67z"/>
      </svg>
    </div>
  );
}