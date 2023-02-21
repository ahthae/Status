using Status.Api.Models;
using Status.Core.Models;

namespace Status.Api.Services
{
    public class ServerConfigurationException : Exception
    {
        public override string Message { get; } = "Server configuration empty or misconfigured.";
    }

    public class ResponseService
    {
        private readonly ILogger<ResponseService> _logger;
        private readonly HttpClient _httpClient;

        public ResponseService(ILogger<ResponseService> logger,
            IConfiguration configuration,
            HttpClient httpClient) 
        {
            _logger = logger;

            var servers = configuration.Get<ServersOptions>().Servers;
            if (servers == null || servers.Count == 0)
            {
                throw new ServerConfigurationException();
            }
            Servers.AddRange(servers);

            //Start request loop
        }

        public List<Server> Servers { get; set; } = new();

        public async Task MeasureServer(Server server)
        {
            // Get start time
            // Send request
            // Stop timer
            // Store response time in server
            throw new NotImplementedException();
        }

        private async Task<bool> SendRequest(Uri uri)
        {
            // Send request
            // Receive response
            // Validate response
            throw new NotImplementedException();
        }
    }
}
