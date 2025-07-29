# TelemetryIngestor Service

A clean architecture implementation for ingesting and retrieving telemetry data using vertical slice architecture.

## Architecture

This service follows **Clean Architecture** principles with **Vertical Slice Architecture** pattern using:

- **TarSoft.Mediator**: Custom mediator pattern implementation
- **FluentResults**: For error handling and result types
- **FluentValidation**: For input validation
- **Entity Framework Core**: For data persistence

## Project Structure

```
TelemetryIngestor/
├── Domain/                     # Domain entities and business logic
│   ├── Entity.cs              # Base entity class
│   └── TelemetryData.cs       # Telemetry domain entity
├── Infrastructure/            # Infrastructure concerns
│   └── TelemetryDbContext.cs  # EF Core database context
├── Common/                    # Shared behaviors and utilities
│   └── Behaviors/
│       └── ValidationBehavior.cs  # FluentValidation pipeline behavior
├── Features/                  # Vertical slices
│   ├── IngestTelemetry/      # Complete feature slice
│   │   ├── IngestTelemetryCommand.cs     # Command and response DTOs
│   │   ├── IngestTelemetryValidator.cs   # FluentValidation rules
│   │   ├── IngestTelemetryHandler.cs     # Command handler
│   │   └── TelemetryController.cs        # API controller
│   └── GetTelemetryData/     # Complete feature slice
│       ├── GetTelemetryDataQuery.cs      # Query and response DTOs
│       ├── GetTelemetryDataValidator.cs  # FluentValidation rules
│       ├── GetTelemetryDataHandler.cs    # Query handler
│       └── TelemetryQueryController.cs   # API controller
└── Program.cs                 # Application entry point and DI configuration
```

## Vertical Slice Architecture

Each feature is organized as a **vertical slice** containing:
- **Command/Query**: Request and response models
- **Validator**: FluentValidation rules
- **Handler**: Business logic implementation
- **Controller**: HTTP API endpoint

This approach provides:
- **High Cohesion**: Related code is grouped together
- **Low Coupling**: Features are independent of each other
- **Easy Testing**: Each slice can be tested in isolation
- **Maintainability**: Changes are localized to specific features

## Key Features

### 1. Clean Architecture Layers

- **Domain**: Contains business entities and rules
- **Application**: Contains use cases (handlers) and interfaces
- **Infrastructure**: Contains data access and external concerns
- **Presentation**: Contains API controllers and DTOs

### 2. CQRS Pattern

- **Commands**: For write operations (IngestTelemetry)
- **Queries**: For read operations (GetTelemetryData)

### 3. Pipeline Behaviors

- **ValidationBehavior**: Automatically validates requests using FluentValidation
- Easily extensible for logging, caching, authorization, etc.

### 4. Error Handling

- Uses **FluentResults** for rich error handling
- Automatic validation error responses
- Structured error messages

## API Endpoints

### POST /api/telemetry/ingest
Ingests new telemetry data.

**Request:**
```json
{
  "deviceId": "GPS_DEVICE_001",
  "latitude": 52.5200,
  "longitude": 13.4050,
  "timestamp": "2025-01-26T12:00:00Z",
  "speed": 45.5,
  "heading": 135.0,
  "additionalData": "{\"battery\": 85}"
}
```

**Response:**
```json
{
  "id": 1,
  "deviceId": "GPS_DEVICE_001",
  "receivedAt": "2025-01-26T12:00:01Z"
}
```

### GET /api/telemetry/{deviceId}
Retrieves telemetry data for a specific device.

**Query Parameters:**
- `fromDate`: Optional start date filter
- `toDate`: Optional end date filter
- `pageNumber`: Page number (default: 1)
- `pageSize`: Page size (default: 50, max: 1000)

**Response:**
```json
{
  "deviceId": "GPS_DEVICE_001",
  "data": [...],
  "totalCount": 100,
  "pageNumber": 1,
  "pageSize": 50
}
```

## Validation Rules

### IngestTelemetry
- DeviceId: Required, max 100 characters
- Latitude: Between -90 and 90 degrees
- Longitude: Between -180 and 180 degrees
- Timestamp: Required, not more than 5 minutes in the future
- Speed: Optional, must be >= 0 if provided
- Heading: Optional, between 0-360 degrees if provided
- AdditionalData: Optional, max 2000 characters

### GetTelemetryData
- DeviceId: Required, max 100 characters
- PageNumber: Must be > 0
- PageSize: Between 1 and 1000
- Date range validation: FromDate <= ToDate, ToDate <= Now

## Running the Service

1. **Prerequisites:**
   - .NET 9.0 SDK
   - SQL Server (LocalDB is configured by default)

2. **Start the service:**
   ```bash
   dotnet run
   ```

3. **Database:**
   - Database is created automatically on first run
   - Connection string is in `appsettings.json`

4. **Testing:**
   - Use the included `TelemetryIngestor.http` file for API testing
   - OpenAPI documentation available at `/swagger` in development

## Adding New Features

To add a new feature slice:

1. Create a new folder under `Features/`
2. Add your command/query, validator, handler, and controller
3. Register handlers in `Program.cs` (or use assembly scanning)

The architecture supports easy extension and follows established patterns for consistency.
