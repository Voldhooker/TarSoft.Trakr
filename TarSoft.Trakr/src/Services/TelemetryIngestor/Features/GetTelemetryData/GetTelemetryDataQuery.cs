using FluentResults;
using TarSoft.Mediator;

namespace TelemetryIngestor.Features.GetTelemetryData;

public record GetTelemetryDataQuery(
    string DeviceId,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 50
) : IRequest<Result<GetTelemetryDataResponse>>;

public record GetTelemetryDataResponse(
    string DeviceId,
    IEnumerable<TelemetryDataDto> Data,
    int TotalCount,
    int PageNumber,
    int PageSize
);

public record TelemetryDataDto(
    int Id,
    string DeviceId,
    double Latitude,
    double Longitude,
    double? Speed,
    double? Heading,
    DateTime Timestamp,
    DateTime ReceivedAt,
    string? AdditionalData
);
