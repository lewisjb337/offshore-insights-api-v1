using System.Text.Json.Serialization;

namespace OffshoreInsights.Application.Features.Geofences.Requests;

public record GetGeofencesRequest
{
    public string UserId { get; init; } = string.Empty;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

public record CreateGeofenceRequest
{
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string ShapeType { get; init; } = string.Empty;  // "polygon" | "circle"
    public string? Colour { get; init; }

    // Polygon — raw GeoJSON geometry object as JSON string, or array of [lon, lat] pairs
    public string? Geometry { get; init; }

    // Circle
    public double? CircleCenterLat { get; init; }
    public double? CircleCenterLon { get; init; }
    public decimal? CircleRadiusKm { get; init; }

    public string[]? VesselTypeFilter { get; init; }
    public bool NotificationsEnabled { get; init; } = true;
    public long? WatchedBuoyMmsi { get; init; }
}

public record DeleteGeofenceRequest(Guid Id, string UserId);

public record GetGeofenceEventsRequest
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = string.Empty;
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
