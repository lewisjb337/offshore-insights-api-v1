using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Geofences.Responses;

public class GeofenceResponse
{
    public GeofenceResponse() { }

    internal GeofenceResponse(Geofence g)
    {
        Id                    = g.Id;
        Name                  = g.Name;
        ShapeType             = g.ShapeType;
        Colour                = g.Colour;
        Geometry              = g.Geometry;
        CircleCenter          = g.CircleCenter;
        CircleRadiusKm        = g.CircleRadiusKm;
        VesselTypeFilter      = g.VesselTypeFilter;
        NotificationsEnabled  = g.NotificationsEnabled;
        WatchedBuoyMmsi       = g.WatchedBuoyMmsi;
        CreatedAt             = g.CreatedAt;
        UpdatedAt             = g.UpdatedAt;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShapeType { get; set; } = string.Empty;
    public string? Colour { get; set; }
    public string Geometry { get; set; } = string.Empty;
    public string? CircleCenter { get; set; }
    public decimal? CircleRadiusKm { get; set; }
    public string[]? VesselTypeFilter { get; set; }
    public bool NotificationsEnabled { get; set; }
    public long? WatchedBuoyMmsi { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public static implicit operator GeofenceResponse(Geofence g) => new(g);
}
