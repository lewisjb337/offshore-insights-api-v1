using System.Text.Json;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Geofences.Responses;

public class GeofenceResponse
{
    public GeofenceResponse() { }

    internal GeofenceResponse(Geofence geofence)
    {
        Id              = geofence.Id;
        Name            = geofence.Name;
        Type            = geofence.Type;
        CenterLatitude  = geofence.CenterLatitude;
        CenterLongitude = geofence.CenterLongitude;
        RadiusMetres    = geofence.RadiusMetres;
        CreatedAt       = geofence.CreatedAt;

        Coordinates = geofence.CoordinatesJson is not null
            ? JsonSerializer.Deserialize<IEnumerable<double[]>>(geofence.CoordinatesJson)
            : null;

        TargetMmsis = geofence.TargetMmsisJson is not null
            ? JsonSerializer.Deserialize<IEnumerable<long>>(geofence.TargetMmsisJson)
            : null;
    }

    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GeofenceType Type { get; set; }
    public double? CenterLatitude { get; set; }
    public double? CenterLongitude { get; set; }
    public double? RadiusMetres { get; set; }
    public IEnumerable<double[]>? Coordinates { get; set; }
    public IEnumerable<long>? TargetMmsis { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public static implicit operator GeofenceResponse(Geofence geofence) => new(geofence);
}
