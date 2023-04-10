using Status.Core.Models;

namespace Status.Infrastructure
{
    public interface IServerRepository
    {
        public IEnumerable<Server> GetServers();

        public Server GetServer(string url);

        public void AddServer(Server server);

        public void AddResponse(Server server, Response response);
    }
}