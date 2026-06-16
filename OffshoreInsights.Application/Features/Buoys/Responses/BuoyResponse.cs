using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Buoys.Responses;

public class BuoyResponse
{
    public BuoyResponse() { }

    protected BuoyResponse(Buoy buoy)
    {
        Id             = buoy.Id;
        Mmsi           = buoy.Mmsi;
        Name           = buoy.Name;
        BuoyType       = buoy.BuoyType;
        AtonTypeCode   = buoy.AtonTypeCode;
        AtonTypeName   = buoy.AtonTypeName;
        VirtualAid     = buoy.VirtualAid;
        OffPosition    = buoy.OffPosition;
        LastSeen       = buoy.LastSeen;
        CreatedAt      = buoy.CreatedAt;
        ProjectName    = buoy.ProjectName;
        SeaArea        = buoy.SeaArea;
        Country        = buoy.Country;
        DeploymentDate = buoy.DeploymentDate;
        RecoveryDate   = buoy.RecoveryDate;
        BuoySubtype    = buoy.BuoySubtype;
        Operator       = buoy.Operator;
    }

    public Guid Id { get; set; }
    public long Mmsi { get; set; }
    public string? Name { get; set; }
    public string BuoyType { get; set; } = "AtoN";
    public int? AtonTypeCode { get; set; }
    public string? AtonTypeName { get; set; }
    public bool VirtualAid { get; set; }
    public bool OffPosition { get; set; }
    public DateTimeOffset? LastSeen { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? ProjectName { get; set; }
    public string? SeaArea { get; set; }
    public string? Country { get; set; }
    public DateOnly? DeploymentDate { get; set; }
    public DateOnly? RecoveryDate { get; set; }
    public string? BuoySubtype { get; set; }
    public string? Operator { get; set; }

    public static implicit operator BuoyResponse(Buoy buoy) => new(buoy);
}
