using OffshoreInsights.Application.Features.Buoys.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Buoys.Abstractions;

public interface IBuoysData
{
    Task<PagedResponse<Buoy>> GetBuoysAsync(GetBuoysRequest request, CancellationToken cancellationToken = default);
    Task<Buoy> GetBuoyPositionByIdAsync(GetBuoyPositionByIdRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponse<BuoyPositionHistory>> GetBuoyTrackByIdAsync(GetBuoyTrackByIdRequest request, CancellationToken cancellationToken = default);
}
