using MediatR;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Responses;

namespace OffshoreInsights.Application.Features.Vessels.Handlers;

public class GetVesselPositionByMmsiHandler(IVesselsData vesselsData) : IRequestHandler<GetVesselPositionByMmsiQuery, VesselPositionResponse>
{
    public async Task<VesselPositionResponse> Handle(GetVesselPositionByMmsiQuery query, CancellationToken cancellationToken)
    {
        var result = await vesselsData.GetVesselPositionByMmsiAsync(query.Request, cancellationToken);

        return result;
    }
}
