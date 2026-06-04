using OffshoreInsights.Application.Features.Geofences.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Geofences.Abstractions;

public interface IGeofencesData
{
    Task<PagedResponse<Geofence>> GetGeofencesAsync(GetGeofencesRequest request, CancellationToken cancellationToken = default);
    Task<Geofence> CreateGeofenceAsync(CreateGeofenceRequest request, CancellationToken cancellationToken = default);
    Task DeleteGeofenceAsync(DeleteGeofenceRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponse<GeofenceEvent>> GetGeofenceEventsAsync(GetGeofenceEventsRequest request, CancellationToken cancellationToken = default);
}
