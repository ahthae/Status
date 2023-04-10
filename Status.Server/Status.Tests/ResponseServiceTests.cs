
using Xunit.Abstractions;
using RichardSzalay.MockHttp;
using Moq;
using Status.Core.Models;
using Status.Infrastructure.Services;
using System.Net;
using Status.Infrastructure.Repositories;

namespace Status.Tests
{
    public class ResponseServiceTests
    {
        public static IEnumerable<object[]> GetServers()
        {
            yield return new object[]
            {
                new List<Server> {
                    new Server() { Id = "1", Url = new("https://test1.status.test"), Responses = new() },
                    new Server() { Id = "2", Url = new("https://test2.status.test"), Responses = new() },
                    new Server() { Id = "3", Url = new("https://test3.status.test"), Responses = new() }
                }
            };
        }

        private static Mock<IServerRepository> MockRepoFactory(IEnumerable<Server> servers)
        {
            var mockRepo = new Mock<IServerRepository>();
            mockRepo.Setup(repo => repo.GetServersAsync()).ReturnsAsync(servers);
            mockRepo.Setup(repo => repo.AddResponseAsync(It.IsAny<string>(), It.IsAny<Response>()))
                .Callback<string, Response>((id, r) => servers.Where(s => s.Id == id).First().Responses.Add(r));
            return mockRepo;
        }

        private readonly ITestOutputHelper _output;
        public ResponseServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(GetServers))]
        public async void Should_CreateNonNullStatusCodeResponse(IEnumerable<Server> servers)
        {
            // Arrange
            var mockRepo = MockRepoFactory(servers);
            var mockHttp = new MockHttpMessageHandler();
            foreach (var server in servers)
                mockHttp.Expect(server.Url.ToString()).Respond("application/json", "{'testkey' : 'testval'}");

            ResponseService service = new(_output.ToLogger<ResponseService>(), mockHttp.ToHttpClient(), mockRepo.Object);

            // Act
            await service.RequestStatusFromAllServers(CancellationToken.None);

            // Assert
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.All(servers, s => Assert.All(s.Responses, r => Assert.NotNull(r.StatusCode)));
        }

        [Theory]
        [MemberData(nameof(GetServers))]
        public async void Should_CreateNonNullStatusCodeResponsesOnHttpStatusFailureCode(IEnumerable<Server> servers)
        {
            // Arrange
            var mockRepo = MockRepoFactory(servers);
            var mockHttp = new MockHttpMessageHandler();
            foreach (var server in servers)
                mockHttp.Expect(server.Url.ToString()).Respond(HttpStatusCode.BadGateway);

            ResponseService service = new(_output.ToLogger<ResponseService>(), mockHttp.ToHttpClient(), mockRepo.Object);

            // Act
            await service.RequestStatusFromAllServers(CancellationToken.None);

            // Assert
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.All(servers, s => Assert.All(s.Responses, r => Assert.NotNull(r.StatusCode)));
        }

        [Theory]
        [MemberData(nameof(GetServers))]
        public async void Should_CreateNullResponsesOnHttpRequestException(IEnumerable<Server> servers)
        {
            // Arrange
            var mockRepo = MockRepoFactory(servers);
            var mockHttp = new MockHttpMessageHandler();
            foreach (var server in servers)
                mockHttp.Expect(server.Url.ToString()).Throw(new HttpRequestException());

            ResponseService service = new(_output.ToLogger<ResponseService>(), mockHttp.ToHttpClient(), mockRepo.Object);

            // Act
            await service.RequestStatusFromAllServers(CancellationToken.None);

            // Assert
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.All(servers, s => Assert.All(s.Responses, r => Assert.Null(r.StatusCode)));
        }
    }
}