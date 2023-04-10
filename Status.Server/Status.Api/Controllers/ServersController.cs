using Microsoft.AspNetCore.Mvc;
using Status.Core.Models;
using Status.Infrastructure.Repositories;
using System.Net.Mime;

namespace Status.Api.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly ILogger<ServersController> _logger;
        private readonly IServerRepository _servers;
        public ServersController(ILogger<ServersController> logger, IServerRepository servers) 
        {
            _logger = logger;
            _servers = servers;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Server>>> Get()
        {
            var res = await _servers.GetServersAsync();
            if (!res.Any()) { return NoContent();  }
            return Ok(res);
        }

        [HttpGet("url")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<Server>>> Get([FromQuery] Uri uri)
        {
            var res = await _servers.GetServersByUri(uri);
            if (!res.Any()) { return NoContent(); }
            return Ok(res);
        }
    }
}
