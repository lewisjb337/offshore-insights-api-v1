using MediatR;
using OffshoreInsights.Application.Features.Buoys.Abstractions;
using OffshoreInsights.Application.Features.Buoys.Queries;
using OffshoreInsights.Application.Features.Buoys.Responses;

namespace OffshoreInsights.Application.Features.Buoys.Handlers;

public class GetBuoyPositionByIdHandler(IBuoysData buoysData) : IRequestHandler<GetBuoyPositionByIdQuery, BuoyPositionResponse>
{
    public async Task<BuoyPositionResponse> Handle(GetBuoyPositionByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await buoysData.GetBuoyPositionByIdAsync(query.Request, cancellationToken);

        return result;
    }
}
