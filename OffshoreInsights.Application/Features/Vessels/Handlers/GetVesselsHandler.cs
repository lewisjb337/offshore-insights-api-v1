using MediatR;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Vessels.Handlers;

public class GetVesselsHandler(IVesselsData vesselsData) : IRequestHandler<GetVesselsQuery, PagedResponse<VesselResponse>>
{
    public async Task<PagedResponse<VesselResponse>> Handle(GetVesselsQuery query, CancellationToken cancellationToken)
    {
        var results = await vesselsData.GetVesselsAsync(query.Request, cancellationToken);

        return new PagedResponse<VesselResponse>
        {
            Items = results.Items.Select(vessel => (VesselResponse)vessel),
            Page = results.Page,
            PageSize = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
