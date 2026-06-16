namespace OffshoreInsights.Domain.Entities;

/// <summary>Maps to geofence_notifications — entry/exit events for a geofence zone.</summary>
public class GeofenceEvent
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? GeofenceId { get; set; }
    public string? GeofenceName { get; set; }
    public long? VesselId { get; set; }
    public string? VesselName { get; set; }
    public string? VesselType { get; set; }
    public string EventType { get; set; } = string.Empty;  // "entered" | "exited"
    public string SourceType { get; set; } = "vessel";
    public DateTimeOffset? EnteredAt { get; set; }
    public DateTimeOffset? ExitedAt { get; set; }
    public int? DurationMinutes { get; set; }
    public bool Read { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? BuoyName { get; set; }
    public long? BuoyMmsi { get; set; }
}
