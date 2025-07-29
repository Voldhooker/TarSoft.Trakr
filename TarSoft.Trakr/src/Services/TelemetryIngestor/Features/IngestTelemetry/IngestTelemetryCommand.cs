using FluentResults;
using TarSoft.Mediator;

namespace TelemetryIngestor.Features.IngestTelemetry;

public record IngestTelemetryCommand(
    string DeviceId,
    double Latitude,
    double Longitude,
    DateTime Timestamp,
    double? Speed = null,
    double? Heading = null,
    string? AdditionalData = null
) : IRequest<Result<IngestTelemetryResponse>>, ICustomCommand;

public record IngestTelemetryResponse(
    int Id,
    string DeviceId,
    DateTime ReceivedAt
);
