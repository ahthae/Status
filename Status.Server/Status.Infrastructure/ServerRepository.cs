using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Status.Core.Models;
using Status.Infrastructure.Models;

namespace Status.Infrastructure
{
    public class ServerRepository : IServerRepository
    {
        private readonly ILogger<ServerRepository> _logger;
        private List<Server> _servers; //TODO persistence layer

        public ServerRepository(ILogger<ServerRepository> logger, IConfiguration configuration) {
            _logger = logger;
            _servers = configuration.Get<StatusOptions>().Servers;
        }

        public IEnumerable<Server> GetServers()
        {
            return _servers;
        }

        public Server GetServer(string url)
        {
            return _servers.Where(server => server.Url.ToString() == url).First();
        }

        public void AddServer(Server server)
        {
            _servers.Add(server);
        }
        public void AddResponse(Server server, Response response)
        {
            server.Responses.Add(response);
        }
    }
}