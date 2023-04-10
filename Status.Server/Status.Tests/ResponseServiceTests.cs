
using Xunit.Abstractions;
using RichardSzalay.MockHttp;
using Status.Infrastructure;
using Moq;
using Status.Core.Models;
using Status.Infrastructure.Services;
using System.Net;

namespace Status.Tests
{
    public class ResponseServiceTests
    {
        public static IEnumerable<object[]> GetServers()
        {
            yield return new object[]
            {
                new List<Server> {
                    new Server() { Url = new("https://test1.status.test"), Responses = new() },
                    new Server() { Url = new("https://test2.status.test"), Responses = new() },
                    new Server() { Url = new("https://test3.status.test"), Responses = new() }
                }
            };
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
            var mockRepo = new Mock<IServerRepository>();
            var mockHttp = new MockHttpMessageHandler();
            mockRepo.Setup(repo => repo.GetServers()).Returns(servers);
            mockRepo.Setup(repo => repo.AddResponse(It.IsAny<Server>(), It.IsAny<Response>()))
                .Callback<Server, Response>((s, r) => s.Responses.Add(r));
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
        public async void Should_CreateNonNullStatusCodeResponsesOnHttpStatusFailureCode(List<Server> servers)
        {
            // Arrange
            var mockRepo = new Mock<IServerRepository>();
            var mockHttp = new MockHttpMessageHandler();
            mockRepo.Setup(repo => repo.GetServers()).Returns(servers);
            mockRepo.Setup(repo => repo.AddResponse(It.IsAny<Server>(), It.IsAny<Response>()))
                .Callback<Server, Response>((s, r) => s.Responses.Add(r));
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
        public async void Should_CreateNullResponsesOnHttpRequestException(List<Server> servers)
        {
            // Arrange
            var mockRepo = new Mock<IServerRepository>();
            var mockHttp = new MockHttpMessageHandler();
            mockRepo.Setup(repo => repo.GetServers()).Returns(servers);
            mockRepo.Setup(repo => repo.AddResponse(It.IsAny<Server>(), It.IsAny<Response>()))
                .Callback<Server, Response>((s, r) => s.Responses.Add(r));
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