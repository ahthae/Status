using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Status.Api.Services;
using Xunit.Abstractions;

namespace Status.Tests;

public class ResponseServiceTests
{
    private readonly ITestOutputHelper _output;

    public ResponseServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void Should_PopulateServersFromConfiguration()
    {
        // Arrange
        List<string> urls = new() { "https://duckduckgo.com/", "https://test.url/api/endpoint?param=value" };
        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Servers:0:Url"] = urls[0],
                ["Servers:1:Url"] = urls[1]
            })
            .Build();

        // Act
        ResponseService responseService = new ResponseService(null, config, null);

        // Assert
        List<string> serviceUrls = (from server in responseService.Servers select server.Url.ToString()).ToList();
        Assert.Equal(urls, serviceUrls);
        
        // Output
        foreach (var server in responseService.Servers)
        {
            _output.WriteLine($"{JsonSerializer.Serialize(server)}");
        }
    }

    [Fact]
    public void Should_ThrowServerConfigurationExceptionOnEmptyServerList()
    {
        // Arrange
        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Servers:0:Url"] = ""
            }).Build();
        
        // Act
        var exception = Assert.Throws<ServerConfigurationException>(() => new ResponseService(null, config, null));

        // Assert
        Assert.IsType<ServerConfigurationException>(exception);
    }
    
    // TODO exception on server configuration validation error
}