using FluentValidation;

namespace TelemetryIngestor.Features.IngestTelemetry;

public class IngestTelemetryValidator : AbstractValidator<IngestTelemetryCommand>
{
    public IngestTelemetryValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("Device ID is required")
            .MaximumLength(100)
            .WithMessage("Device ID cannot exceed 100 characters");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees");

        RuleFor(x => x.Timestamp)
            .NotEmpty()
            .WithMessage("Timestamp is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
            .WithMessage("Timestamp cannot be more than 5 minutes in the future");

        When(x => x.Speed.HasValue, () =>
        {
            RuleFor(x => x.Speed!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Speed cannot be negative");
        });

        When(x => x.Heading.HasValue, () =>
        {
            RuleFor(x => x.Heading!.Value)
                .InclusiveBetween(0, 360)
                .WithMessage("Heading must be between 0 and 360 degrees");
        });

        When(x => !string.IsNullOrEmpty(x.AdditionalData), () =>
        {
            RuleFor(x => x.AdditionalData!)
                .MaximumLength(2000)
                .WithMessage("Additional data cannot exceed 2000 characters");
        });
    }
}
