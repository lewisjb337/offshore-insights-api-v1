using MediatR;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Vessels.Handlers;

public class GetVesselTrackByMmsiHandler(IVesselsData vesselsData) : IRequestHandler<GetVesselTrackByMmsiQuery, PagedResponse<VesselTrackPointResponse>>
{
    public async Task<PagedResponse<VesselTrackPointResponse>> Handle(GetVesselTrackByMmsiQuery query, CancellationToken cancellationToken)
    {
        var results = await vesselsData.GetVesselTrackByMmsiAsync(query.Request, cancellationToken);

        return new PagedResponse<VesselTrackPointResponse>
        {
            Items      = results.Items.Select(h => new VesselTrackPointResponse(h)),
            Page       = results.Page,
            PageSize   = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
