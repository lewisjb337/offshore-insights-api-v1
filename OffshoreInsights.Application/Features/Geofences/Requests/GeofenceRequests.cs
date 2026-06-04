using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Geofences.Requests;

/// <summary>
/// Retrieves a paginated list of all geofences for the account.
/// </summary>
public record GetGeofencesRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

/// <summary>
/// Creates a new geofence zone.
/// Supply <see cref="CenterLatitude"/>, <see cref="CenterLongitude"/>, and <see cref="RadiusMetres"/>
/// for a circle, or <see cref="Coordinates"/> (array of [latitude, longitude] pairs) for a polygon.
/// </summary>
public record CreateGeofenceRequest
{
    public string Name { get; init; } = string.Empty;
    public GeofenceType Type { get; init; }

    // Circle
    public double? CenterLatitude { get; init; }
    public double? CenterLongitude { get; init; }
    public double? RadiusMetres { get; init; }

    // Polygon — list of [latitude, longitude] pairs
    public IEnumerable<double[]>? Coordinates { get; init; }

    // Target vessels by MMSI
    public IEnumerable<long>? TargetMmsis { get; init; }
}

/// <summary>
/// Deletes a geofence by its ID.
/// </summary>
public record DeleteGeofenceRequest(long Id);

/// <summary>
/// Retrieves paginated entry/exit events for a geofence over an optional time range.
/// </summary>
public record GetGeofenceEventsRequest
{
    public long Id { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
