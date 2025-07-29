using FluentResults;
using Microsoft.EntityFrameworkCore;
using TarSoft.Mediator;
using TelemetryIngestor.Domain;
using TelemetryIngestor.Infrastructure;

namespace TelemetryIngestor.Features.IngestTelemetry;

public class IngestTelemetryHandler : ICommandHandler<IngestTelemetryCommand, Result<IngestTelemetryResponse>>
{
    private readonly TelemetryDbContext _context;

    public IngestTelemetryHandler(TelemetryDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IngestTelemetryResponse>> Handle(IngestTelemetryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var telemetryData = new TelemetryData(
                command.DeviceId,
                command.Latitude,
                command.Longitude,
                command.Timestamp,
                command.Speed,
                command.Heading,
                command.AdditionalData
            );

            _context.TelemetryData.Add(telemetryData);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new IngestTelemetryResponse(
                telemetryData.Id,
                telemetryData.DeviceId,
                telemetryData.ReceivedAt
            );

            return Result.Ok(response);
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Unexpected error: {ex.Message}");
        }
    }
}
