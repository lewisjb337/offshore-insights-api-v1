namespace OffshoreInsights.Domain.Entities;

public class Geofence
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Colour { get; set; }
    public string ShapeType { get; set; } = string.Empty;   // "polygon" | "circle"
    public string Geometry { get; set; } = string.Empty;    // jsonb
    public string? CircleCenter { get; set; }               // jsonb
    public decimal? CircleRadiusKm { get; set; }
    public string[]? VesselTypeFilter { get; set; }
    public bool NotificationsEnabled { get; set; } = true;
    public long? WatchedBuoyMmsi { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
