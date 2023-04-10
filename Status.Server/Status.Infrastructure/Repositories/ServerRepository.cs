using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Status.Core.Models;
using Status.Infrastructure.Models;

namespace Status.Infrastructure.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly ILogger<ServerRepository> _logger;
        private readonly IMongoCollection<Server> _servers;

        public ServerRepository(
            ILogger<ServerRepository> logger,
            MongoClient db,
            IOptions<DatabaseSettings> options)
        {
            _logger = logger;
            _servers = db.GetDatabase(options.Value.DatabaseName)
                .GetCollection<Server>(options.Value.ServersCollectionName);
        }

        public async Task<IEnumerable<Server>> GetServersAsync()
        {
            return await _servers.Find(_ => true).ToListAsync();
        }

        public async Task<Server> GetServerAsync(string id)
        {
            return await _servers.Find(s => s.Id == id).SingleAsync();
        }

        public async Task<IEnumerable<Server>> GetServersByUri(Uri url)
        {
            return await _servers.Find(s => s.Url.Equals(url)).ToListAsync();
        }

        public async Task AddServerAsync(Server server)
        {
            await _servers.InsertOneAsync(server);
        }

        public async Task UpdateServer(Server server)
        {
            await _servers.ReplaceOneAsync(s => s.Id == server.Id, server);
        }

        public async Task AddResponseAsync(string id, Response response)
        {
            var server = await _servers.Find(s => s.Id == id).SingleAsync();
            server.Responses.Add(response);
            await UpdateServer(server);
        }
    }
}