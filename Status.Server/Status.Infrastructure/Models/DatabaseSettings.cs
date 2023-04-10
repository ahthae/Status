namespace Status.Infrastructure.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string IncidentsCollectionName { get; set; } = null!;
        public string ServersCollectionName { get; set; } = null!;
    }
}
