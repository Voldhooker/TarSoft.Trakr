using Microsoft.AspNetCore.Mvc;
using TarSoft.Mediator;

namespace TelemetryIngestor.Features.GetTelemetryData;

[ApiController]
[Route("api/telemetry")]
public class TelemetryQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public TelemetryQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetTelemetryData(
        string deviceId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTelemetryDataQuery(deviceId, fromDate, toDate, pageNumber, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Message) });
        }

        return Ok(result.Value);
    }
}
