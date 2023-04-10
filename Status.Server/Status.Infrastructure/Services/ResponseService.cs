using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Status.Core.Models;
using System.Diagnostics;

namespace Status.Infrastructure.Services
{
    public class ResponseService : IResponseService
    {
        private readonly ILogger<ResponseService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IServerRepository _serverRepository;

        public ResponseService(
            ILogger<ResponseService> logger, 
            HttpClient httpClient,
            IServerRepository serverRepository)
        {
            _logger = logger;
            _httpClient = httpClient;
            _serverRepository = serverRepository;
        }

        public async Task RequestStatusFromAllServers(CancellationToken cancellationToken)
        {
            Dictionary<Server, Response> responses = new();
            List<Task<(Server, HttpResponseMessage?, TimeSpan)>> tasks = new();

            foreach (var server in _serverRepository.GetServers())
            {
                responses.Add(server, new Response()
                {
                    Timestamp = DateTime.Now,
                    Success = true
                });
            }

            // Start all async requests at the same time
            foreach (var response in responses)
            {
                tasks.Add(SendRequest(response.Key, cancellationToken));
            }

            // Handle requests as soon as they finish for accurate timestamps
            while (tasks.Count > 0)
            {
                int i = Task.WaitAny(tasks.ToArray());
                (Server, HttpResponseMessage?, TimeSpan) task = await tasks[i];

                Server server = task.Item1;
                Response response = responses[server];
                response.ResponseTime = task.Item3;

                HttpResponseMessage? httpResponse = task.Item2;
                if (httpResponse is null || !httpResponse.IsSuccessStatusCode)
                {
                    response.Success = false;
                }
                response.Information = httpResponse?.ReasonPhrase;

                _serverRepository.AddResponse(server, response);

                tasks.Remove(tasks[i]);
            }
        }
        private async Task<(Server, HttpResponseMessage?, TimeSpan)> SendRequest(Server server, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Querying {server}", server.Url);
            HttpResponseMessage? httpResponse = null;
            Stopwatch responseTimer = Stopwatch.StartNew();
            try
            {
                httpResponse = await _httpClient.GetAsync(server.Url, stoppingToken).ConfigureAwait(false);
            }
            catch (HttpRequestException ex) { }
            responseTimer.Stop();

            return (server, httpResponse, responseTimer.Elapsed);

        }
    }
}
