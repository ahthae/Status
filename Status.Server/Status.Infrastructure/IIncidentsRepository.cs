using Status.Core.Models;

namespace Status.Infrastructure
{
    public interface IIncidentsRepository
    {
        public Task<IEnumerable<Incident>> GetIncidentsAsync();

        public Task<Incident> GetIncidentAsync(string id);

        public Task CreateAsync(Incident incident);

        public Task UpdateAsync(string id, Incident incident);

        public Task RemoveAsync(string id);
    }
}