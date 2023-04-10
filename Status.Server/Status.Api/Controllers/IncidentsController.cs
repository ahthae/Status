using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Status.Core.Models;
using Status.Infrastructure.Repositories;

namespace Status.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly ILogger<IncidentsController> _logger;
    private readonly IIncidentsRepository _incidentsRepository;

    public IncidentsController(ILogger<IncidentsController> logger,
        IIncidentsRepository incidentsRepository)
    {
        _logger = logger;
        _incidentsRepository = incidentsRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Incident>>> Get()
    {
        var res = await _incidentsRepository.GetIncidentsAsync();

        if (!res.Any()) return NoContent();

        return Ok(res);
    }

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Incident>>> Get([FromRoute] string id)
    {
        var res = await _incidentsRepository.GetIncidentAsync(id);

        if (res is null) return NotFound();

        return Ok(res);
    }

    [HttpGet("after")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Incident>>> Get([FromQuery] DateTime after)
    {
        var res = await _incidentsRepository.GetIncidentsAfter(after);

        if (!res.Any()) return NoContent();

        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Incident incident)
    {
        await _incidentsRepository.CreateAsync(incident);

        return CreatedAtAction(nameof(Get), new { id = incident.Id }, incident);
    }

    [HttpPatch("{id:length(24)}")]
    public async Task<ActionResult> Update([FromRoute] string id, [FromBody] JsonPatchDocument<Incident> incidentPatch)
    {
        var incident = await _incidentsRepository.GetIncidentAsync(id);

        if (incident is null) return NotFound();

        incidentPatch.ApplyTo(incident);
        await _incidentsRepository.UpdateAsync(id, incident);

        return NoContent();
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> Replace([FromRoute] string id, [FromBody] Incident updatedIncident)
    {
        var incident = await _incidentsRepository.GetIncidentAsync(id);

        if (incident is null) return NotFound();

        updatedIncident.Id = incident.Id;

        await _incidentsRepository.UpdateAsync(id, updatedIncident);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        await _incidentsRepository.RemoveAsync(id);
        return Ok();
    }
}
