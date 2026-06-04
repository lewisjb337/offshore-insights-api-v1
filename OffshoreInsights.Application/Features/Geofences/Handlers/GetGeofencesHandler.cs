using MediatR;
using OffshoreInsights.Application.Features.Geofences.Abstractions;
using OffshoreInsights.Application.Features.Geofences.Queries;
using OffshoreInsights.Application.Features.Geofences.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Geofences.Handlers;

public class GetGeofencesHandler(IGeofencesData geofencesData) : IRequestHandler<GetGeofencesQuery, PagedResponse<GeofenceResponse>>
{
    public async Task<PagedResponse<GeofenceResponse>> Handle(GetGeofencesQuery query, CancellationToken cancellationToken)
    {
        var results = await geofencesData.GetGeofencesAsync(query.Request, cancellationToken);

        return new PagedResponse<GeofenceResponse>
        {
            Items      = results.Items.Select(g => (GeofenceResponse)g),
            Page       = results.Page,
            PageSize   = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
