using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Status.Core.Models;
using Status.Infrastructure.Models;
using System.Diagnostics;

namespace Status.Infrastructure.Workers
{
    public class StatusWorker : BackgroundService
    {
        private readonly ILogger<StatusWorker> _logger;
        private readonly HttpClient _httpClient;
        private readonly IServerRepository _serverRepository;
        private readonly PeriodicTimer _timer;

        public StatusWorker(ILogger<StatusWorker> logger, IConfiguration configuration, IServerRepository serverRepository, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _serverRepository = serverRepository;

            _timer = new PeriodicTimer(configuration.Get<StatusOptions>().Rate);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
            {
                Dictionary<Server, Response> responses = new();
                List<Task<(Server, HttpResponseMessage ?,TimeSpan)>> tasks = new();

                foreach (var server in _serverRepository.GetServers())
                {
                    Response response = new Response()
                    {
                        Timestamp = DateTime.Now,
                        Success = true
                    };

                    responses.Add(server, response);
                }

                foreach (var response in responses)
                {
                    tasks.Add(SendRequest(response.Key, stoppingToken));
                }

                while(tasks.Count > 0)
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

                    server.Responses.Add(response);

                    tasks.Remove(tasks[i]);
                }
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
