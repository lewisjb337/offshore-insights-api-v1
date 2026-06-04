using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Geofences.Responses;

public class GeofenceEventResponse
{
    public GeofenceEventResponse() { }

    internal GeofenceEventResponse(GeofenceEvent geofenceEvent)
    {
        Id          = geofenceEvent.Id;
        GeofenceId  = geofenceEvent.GeofenceId;
        EventType   = geofenceEvent.EventType;
        Mmsi        = geofenceEvent.Mmsi;
        BuoyId      = geofenceEvent.BuoyId;
        OccurredAt  = geofenceEvent.OccurredAt;
        Latitude    = geofenceEvent.Latitude;
        Longitude   = geofenceEvent.Longitude;
    }

    public long Id { get; set; }
    public long GeofenceId { get; set; }
    public GeofenceEventType EventType { get; set; }
    public long? Mmsi { get; set; }
    public long? BuoyId { get; set; }
    public DateTime OccurredAt { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public static implicit operator GeofenceEventResponse(GeofenceEvent geofenceEvent) => new(geofenceEvent);
}
