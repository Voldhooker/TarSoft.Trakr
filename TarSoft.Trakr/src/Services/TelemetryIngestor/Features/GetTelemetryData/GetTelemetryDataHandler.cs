using FluentResults;
using Microsoft.EntityFrameworkCore;
using TarSoft.Mediator;
using TelemetryIngestor.Infrastructure;

namespace TelemetryIngestor.Features.GetTelemetryData;

public class GetTelemetryDataHandler : IQueryHandler<GetTelemetryDataQuery, Result<GetTelemetryDataResponse>>
{
    private readonly TelemetryDbContext _context;

    public GetTelemetryDataHandler(TelemetryDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetTelemetryDataResponse>> Handle(GetTelemetryDataQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var queryable = _context.TelemetryData
                .Where(x => x.DeviceId == query.DeviceId);

            if (query.FromDate.HasValue)
            {
                queryable = queryable.Where(x => x.Timestamp >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                queryable = queryable.Where(x => x.Timestamp <= query.ToDate.Value);
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var data = await queryable
                .OrderByDescending(x => x.Timestamp)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new TelemetryDataDto(
                    x.Id,
                    x.DeviceId,
                    x.Latitude,
                    x.Longitude,
                    x.Speed,
                    x.Heading,
                    x.Timestamp,
                    x.ReceivedAt,
                    x.AdditionalData
                ))
                .ToListAsync(cancellationToken);

            var response = new GetTelemetryDataResponse(
                query.DeviceId,
                data,
                totalCount,
                query.PageNumber,
                query.PageSize
            );

            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error retrieving telemetry data: {ex.Message}");
        }
    }
}
