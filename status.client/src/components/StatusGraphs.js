import '../styles/StatusGraphs.module.scss'
import StatusGraph from "./StatusGraph";

export default function StatusGraphs({ servers }) {
  const graphs = servers.map(server => 
    <StatusGraph key={server.id} server={server} />
  );

  return (
    <>
        {graphs}
    </>
  );
}