using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Buoys.Responses;

public class BuoyPositionResponse : BuoyResponse
{
    public BuoyPositionResponse() { }

    private BuoyPositionResponse(Buoy buoy) : base(buoy)
    {
        CurrentPosition = buoy.CurrentPosition is not null
            ? new BuoyTelemetryResponse(buoy.CurrentPosition)
            : null;
    }

    public BuoyTelemetryResponse? CurrentPosition { get; set; }

    public static implicit operator BuoyPositionResponse(Buoy buoy) => new(buoy);
}
