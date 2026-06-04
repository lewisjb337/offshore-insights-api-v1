using MediatR;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Responses;

namespace OffshoreInsights.Application.Features.Vessels.Handlers;

public class GetVesselByMmsiHandler(IVesselsData vesselsData) : IRequestHandler<GetVesselByMmsiQuery, VesselResponse>
{
    public async Task<VesselResponse> Handle(GetVesselByMmsiQuery query, CancellationToken cancellationToken)
    {
        var result = await vesselsData.GetVesselByMmsiAsync(query.Request, cancellationToken);

        return result;
    }
}
