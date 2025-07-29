namespace TelemetryIngestor.Domain;

public class TelemetryData : Entity
{
    public string DeviceId { get; private set; } = string.Empty;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double? Speed { get; private set; }
    public double? Heading { get; private set; }
    public DateTime Timestamp { get; private set; }
    public DateTime ReceivedAt { get; private set; }
    public string? AdditionalData { get; private set; }

    private TelemetryData() { } // For EF Core

    public TelemetryData(
        string deviceId,
        double latitude,
        double longitude,
        DateTime timestamp,
        double? speed = null,
        double? heading = null,
        string? additionalData = null)
    {
        DeviceId = deviceId;
        Latitude = latitude;
        Longitude = longitude;
        Timestamp = timestamp;
        Speed = speed;
        Heading = heading;
        AdditionalData = additionalData;
        ReceivedAt = DateTime.UtcNow;
    }

    public void UpdatePosition(double latitude, double longitude, DateTime timestamp)
    {
        Latitude = latitude;
        Longitude = longitude;
        Timestamp = timestamp;
    }
}
