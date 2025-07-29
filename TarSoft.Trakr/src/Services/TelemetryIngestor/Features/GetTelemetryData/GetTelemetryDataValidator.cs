using FluentValidation;

namespace TelemetryIngestor.Features.GetTelemetryData;

public class GetTelemetryDataValidator : AbstractValidator<GetTelemetryDataQuery>
{
    public GetTelemetryDataValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("Device ID is required")
            .MaximumLength(100)
            .WithMessage("Device ID cannot exceed 100 characters");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 1000)
            .WithMessage("Page size must be between 1 and 1000");

        When(x => x.FromDate.HasValue && x.ToDate.HasValue, () =>
        {
            RuleFor(x => x.FromDate!.Value)
                .LessThanOrEqualTo(x => x.ToDate!.Value)
                .WithMessage("From date must be less than or equal to To date");
        });

        When(x => x.ToDate.HasValue, () =>
        {
            RuleFor(x => x.ToDate!.Value)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("To date cannot be in the future");
        });
    }
}
