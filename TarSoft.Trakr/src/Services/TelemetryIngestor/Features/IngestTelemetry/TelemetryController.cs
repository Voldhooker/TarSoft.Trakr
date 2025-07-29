using Microsoft.AspNetCore.Mvc;
using TarSoft.Mediator;
using TelemetryIngestor.Features.IngestTelemetry;

namespace TelemetryIngestor.Features.IngestTelemetry;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly IMediator _mediator;

    public TelemetryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> IngestTelemetry([FromBody] IngestTelemetryRequest request, CancellationToken cancellationToken)
    {
        var command = new IngestTelemetryCommand(
            request.DeviceId,
            request.Latitude,
            request.Longitude,
            request.Timestamp,
            request.Speed,
            request.Heading,
            request.AdditionalData
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Message) });
        }

        return Ok(result.Value);
    }
}

public record IngestTelemetryRequest(
    string DeviceId,
    double Latitude,
    double Longitude,
    DateTime Timestamp,
    double? Speed = null,
    double? Heading = null,
    string? AdditionalData = null
);
