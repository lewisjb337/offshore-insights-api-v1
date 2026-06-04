using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Buoys.Responses;

public class BuoyResponse
{
    public BuoyResponse() { }

    protected BuoyResponse(Buoy buoy)
    {
        Id        = buoy.Id;
        Name      = buoy.Name;
        Owner     = buoy.Owner;
        Country   = buoy.Country;
        IsActive  = buoy.IsActive;
        FirstSeen = buoy.FirstSeen;
        LastSeen  = buoy.LastSeen;
    }

    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Owner { get; set; }
    public string? Country { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? FirstSeen { get; set; }
    public DateTime? LastSeen { get; set; }

    public static implicit operator BuoyResponse(Buoy buoy) => new(buoy);
}
