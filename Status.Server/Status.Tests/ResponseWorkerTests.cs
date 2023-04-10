
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using RichardSzalay.MockHttp;
using Status.Infrastructure.Workers;
using Status.Infrastructure;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Status.Infrastructure.Models;
using Status.Core.Models;
using Microsoft.Extensions.Hosting;

namespace Status.Tests
{
    public class ResponseWorkerTests
    {
        private readonly ITestOutputHelper _output;
        public ResponseWorkerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void Should_CreateSuccessResponse()
        {
            // Arrange
            Uri uri = new("https://status.test");
            List<Server> servers = new()
            { 
                new Server()
                { 
                    Url = uri, 
                    Responses = new() 
                } 
            };

            var mockRepo = new Mock<IServerRepository>();
            mockRepo.Setup(repo => repo.GetServers()).Returns(servers);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(uri.ToString()).Respond("application/json", "{'testkey' : 'testval'}");

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Rate"] = "00:00:01"
                }).Build();

            var services = new ServiceCollection()
                .Configure<StatusOptions>(configuration)
                .AddLogging(builder => builder.AddXUnit())
                .AddSingleton(mockRepo.Object)
                .AddSingleton(mockHttp.ToHttpClient())
                .AddHostedService<ResponseWorker>();

            var serviceProvider = services.BuildServiceProvider();
            var worker = serviceProvider.GetRequiredService<IHostedService>() as ResponseWorker;

            // Act
            CancellationTokenSource cts = new();
            await worker.StartAsync(cts.Token);
            await Task.Delay(2000);
            cts.Cancel();

            // Assert
            Assert.True(servers.First().Responses.First().Success);
        }

        [Fact]
        public void Should_CreateFailureResponse()
        {

        }
    }
}