using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Domain.Entities;

public class Geofence
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GeofenceType Type { get; set; }

    // Circle geometry
    public double? CenterLatitude { get; set; }
    public double? CenterLongitude { get; set; }
    public double? RadiusMetres { get; set; }

    // Polygon geometry — stored as a JSON array of [latitude, longitude] pairs
    public string? CoordinatesJson { get; set; }

    // Target entities — stored as a JSON array of MMSI numbers
    public string? TargetMmsisJson { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<GeofenceEvent> Events { get; set; } = [];
}
