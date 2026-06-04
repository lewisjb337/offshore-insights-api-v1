using OffshoreInsights.Application.Features.Vessels.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Vessels.Abstractions;

public interface IVesselsData
{
    // ─── Vessel Details ───────────────────────────────────────────────────────

    Task<Vessel> GetVesselByMmsiAsync(GetVesselByMmsiRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponse<Vessel>> GetVesselsAsync(GetVesselsRequest request, CancellationToken cancellationToken = default);

    // ─── Vessel Track ─────────────────────────────────────────────────────────

    Task<PagedResponse<VesselPositionHistory>> GetVesselTrackByMmsiAsync(GetVesselTrackByMmsiRequest request, CancellationToken cancellationToken = default);

    // ─── Vessel Positions ─────────────────────────────────────────────────────

    Task<Vessel> GetVesselPositionByMmsiAsync(GetVesselPositionByMmsiRequest request, CancellationToken cancellationToken = default);
    Task<PagedResponse<Vessel>> GetVesselPositionsAsync(GetVesselPositionsRequest request, CancellationToken cancellationToken = default);
}
