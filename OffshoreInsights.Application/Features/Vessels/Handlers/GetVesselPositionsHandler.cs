using MediatR;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Vessels.Handlers;

public class GetVesselPositionsHandler(IVesselsData vesselsData) : IRequestHandler<GetVesselPositionsQuery, PagedResponse<VesselPositionResponse>>
{
    public async Task<PagedResponse<VesselPositionResponse>> Handle(GetVesselPositionsQuery query, CancellationToken cancellationToken)
    {
        var results = await vesselsData.GetVesselPositionsAsync(query.Request, cancellationToken);

        return new PagedResponse<VesselPositionResponse>
        {
            Items = results.Items.Select(vessel => (VesselPositionResponse)vessel),
            Page = results.Page,
            PageSize = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
