using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Status.Core.Models;
using Status.Infrastructure.Models;

namespace Status.Infrastructure
{
    public class IncidentsRepository : IIncidentsRepository
    {
        private readonly IMongoCollection<Incident> _incidents;
        public IncidentsRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _incidents = new MongoClient(databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName)
                                                                                 .GetCollection<Incident>(databaseSettings.Value.IncidentsCollectionName);
        }

        public async Task<IEnumerable<Incident>> GetIncidentsAsync()
        {
            return await _incidents.Find(_ => true).ToListAsync();
        }

        public async Task<Incident> GetIncidentAsync(string id)
        {
            return await _incidents.Find(x => x.Id == id).FirstOrDefaultAsync();
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