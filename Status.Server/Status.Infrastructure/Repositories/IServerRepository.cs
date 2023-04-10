using Status.Core.Models;

namespace Status.Infrastructure.Repositories
{
    public interface IServerRepository
    {
        public Task<IEnumerable<Server>> GetServersAsync();

        public Task<Server> GetServerAsync(string id);
        public Task<IEnumerable<Server>> GetServersByUri(Uri url);

        public Task AddServerAsync(Server server);

        public Task UpdateServer(Server server);

        public Task AddResponseAsync(string id, Response response);
    }
}