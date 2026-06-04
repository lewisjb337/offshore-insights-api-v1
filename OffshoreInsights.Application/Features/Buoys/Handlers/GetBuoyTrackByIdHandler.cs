using MediatR;
using OffshoreInsights.Application.Features.Buoys.Abstractions;
using OffshoreInsights.Application.Features.Buoys.Queries;
using OffshoreInsights.Application.Features.Buoys.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Buoys.Handlers;

public class GetBuoyTrackByIdHandler(IBuoysData buoysData) : IRequestHandler<GetBuoyTrackByIdQuery, PagedResponse<BuoyTelemetryResponse>>
{
    public async Task<PagedResponse<BuoyTelemetryResponse>> Handle(GetBuoyTrackByIdQuery query, CancellationToken cancellationToken)
    {
        var results = await buoysData.GetBuoyTrackByIdAsync(query.Request, cancellationToken);

        return new PagedResponse<BuoyTelemetryResponse>
        {
            Items      = results.Items.Select(h => new BuoyTelemetryResponse(h)),
            Page       = results.Page,
            PageSize   = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
