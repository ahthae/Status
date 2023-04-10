using Status.Core.Models;

namespace Status.Infrastructure
{
    public interface IServerRepository
    {
        public Task<IEnumerable<Server>> GetServersAsync();

        public Task<Server> GetServerAsync(string id);

        public Task AddServerAsync(Server server);

        public Task UpdateServer(Server server);

        public Task AddResponseAsync(string id, Response response);
    }
}