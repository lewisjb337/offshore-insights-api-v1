using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Geofences.Responses;

public class GeofenceEventResponse
{
    public GeofenceEventResponse() { }

    internal GeofenceEventResponse(GeofenceEvent e)
    {
        Id              = e.Id;
        GeofenceId      = e.GeofenceId;
        GeofenceName    = e.GeofenceName;
        EventType       = e.EventType;
        SourceType      = e.SourceType;
        VesselId        = e.VesselId;
        VesselName      = e.VesselName;
        VesselType      = e.VesselType;
        BuoyMmsi        = e.BuoyMmsi;
        BuoyName        = e.BuoyName;
        EnteredAt       = e.EnteredAt;
        ExitedAt        = e.ExitedAt;
        DurationMinutes = e.DurationMinutes;
        CreatedAt       = e.CreatedAt;
    }

    public Guid Id { get; set; }
    public Guid? GeofenceId { get; set; }
    public string? GeofenceName { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public long? VesselId { get; set; }
    public string? VesselName { get; set; }
    public string? VesselType { get; set; }
    public long? BuoyMmsi { get; set; }
    public string? BuoyName { get; set; }
    public DateTimeOffset? EnteredAt { get; set; }
    public DateTimeOffset? ExitedAt { get; set; }
    public int? DurationMinutes { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public static implicit operator GeofenceEventResponse(GeofenceEvent e) => new(e);
}
