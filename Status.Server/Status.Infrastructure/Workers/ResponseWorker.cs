using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Status.Infrastructure.Models;
using Status.Infrastructure.Services;

namespace Status.Infrastructure.Workers
{
    public class ResponseWorker : BackgroundService
    {
        private readonly ILogger<ResponseWorker> _logger;
        private readonly PeriodicTimer _timer;
        private readonly IResponseService _responseService;

        public ResponseWorker(
            ILogger<ResponseWorker> logger,
            IOptions<StatusOptions> options,
            IResponseService responseService)
        {
            _logger = logger;
            _timer = new PeriodicTimer(options.Value.Rate);
            _responseService = responseService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(cancellationToken))
            {
                await _responseService.RequestStatusFromAllServers(cancellationToken);
            }
        }
    }
}
