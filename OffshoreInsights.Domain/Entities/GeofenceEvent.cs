using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Domain.Entities;

public class GeofenceEvent
{
    public long Id { get; set; }
    public long GeofenceId { get; set; }
    public GeofenceEventType EventType { get; set; }
    public long? Mmsi { get; set; }
    public long? BuoyId { get; set; }
    public DateTime OccurredAt { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public Geofence? Geofence { get; set; }
}
