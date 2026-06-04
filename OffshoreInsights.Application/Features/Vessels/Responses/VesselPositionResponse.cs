using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Vessels.Responses;

public class VesselPositionResponse : VesselResponse
{
    public VesselPositionResponse() { }

    private VesselPositionResponse(Vessel vessel) : base(vessel)
    {
        CurrentPosition = vessel.CurrentPosition is not null
            ? new CurrentPositionResponse(vessel.CurrentPosition)
            : null;
    }

    public CurrentPositionResponse? CurrentPosition { get; set; }

    public static implicit operator VesselPositionResponse(Vessel vessel) => new(vessel);
}
