using MediatR;
using OffshoreInsights.Application.Features.Geofences.Abstractions;
using OffshoreInsights.Application.Features.Geofences.Queries;
using OffshoreInsights.Application.Features.Geofences.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Geofences.Handlers;

public class GetGeofenceEventsHandler(IGeofencesData geofencesData) : IRequestHandler<GetGeofenceEventsQuery, PagedResponse<GeofenceEventResponse>>
{
    public async Task<PagedResponse<GeofenceEventResponse>> Handle(GetGeofenceEventsQuery query, CancellationToken cancellationToken)
    {
        var results = await geofencesData.GetGeofenceEventsAsync(query.Request, cancellationToken);

        return new PagedResponse<GeofenceEventResponse>
        {
            Items      = results.Items.Select(e => (GeofenceEventResponse)e),
            Page       = results.Page,
            PageSize   = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
