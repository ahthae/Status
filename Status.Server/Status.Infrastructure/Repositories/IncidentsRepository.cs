using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Status.Core.Models;
using Status.Infrastructure.Models;

namespace Status.Infrastructure.Repositories
{
    public class IncidentsRepository : IIncidentsRepository
    {
        private readonly ILogger<IncidentsRepository> _logger;
        private readonly IMongoCollection<Incident> _incidents;
        public IncidentsRepository(
            ILogger<IncidentsRepository> logger,
            MongoClient db,
            IOptions<DatabaseSettings> options)
        {
            _logger = logger;
            _incidents = db.GetDatabase(options.Value.DatabaseName)
                .GetCollection<Incident>(options.Value.IncidentsCollectionName);
        }

        public async Task<IEnumerable<Incident>> GetIncidentsAsync()
        {
            return await _incidents.Find(_ => true).ToListAsync();
        }

        public async Task<Incident> GetIncidentAsync(string id)
        {
            return await _incidents.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Incident>> GetIncidentsAfter(DateTime dateTime)
        {
            return await _incidents.Find(x => x.Start > dateTime).ToListAsync();
        }

        public async Task CreateAsync(Incident incident)
        {
            await _incidents.InsertOneAsync(incident);
        }

        public async Task UpdateAsync(string id, Incident incident)
        {
            await _incidents.ReplaceOneAsync(x => x.Id == id, incident);
        }

        public async Task RemoveAsync(string id)
        {
            await _incidents.DeleteOneAsync(x => x.Id == id);
        }
    }
}