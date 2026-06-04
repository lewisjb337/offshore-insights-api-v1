using MediatR;
using OffshoreInsights.Application.Features.Vessels.Requests;
using OffshoreInsights.Application.Features.Vessels.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Vessels.Queries;

public record GetVesselByMmsiQuery(GetVesselByMmsiRequest Request) : IRequest<VesselResponse>;
public record GetVesselsQuery(GetVesselsRequest Request) : IRequest<PagedResponse<VesselResponse>>;
public record GetVesselTrackByMmsiQuery(GetVesselTrackByMmsiRequest Request) : IRequest<PagedResponse<VesselTrackPointResponse>>;
public record GetVesselPositionByMmsiQuery(GetVesselPositionByMmsiRequest Request) : IRequest<VesselPositionResponse>;
public record GetVesselPositionsQuery(GetVesselPositionsRequest Request) : IRequest<PagedResponse<VesselPositionResponse>>;
