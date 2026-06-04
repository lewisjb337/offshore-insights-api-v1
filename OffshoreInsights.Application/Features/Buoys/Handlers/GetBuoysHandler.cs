using MediatR;
using OffshoreInsights.Application.Features.Buoys.Abstractions;
using OffshoreInsights.Application.Features.Buoys.Queries;
using OffshoreInsights.Application.Features.Buoys.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Buoys.Handlers;

public class GetBuoysHandler(IBuoysData buoysData) : IRequestHandler<GetBuoysQuery, PagedResponse<BuoyResponse>>
{
    public async Task<PagedResponse<BuoyResponse>> Handle(GetBuoysQuery query, CancellationToken cancellationToken)
    {
        var results = await buoysData.GetBuoysAsync(query.Request, cancellationToken);

        return new PagedResponse<BuoyResponse>
        {
            Items      = results.Items.Select(buoy => (BuoyResponse)buoy),
            Page       = results.Page,
            PageSize   = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
