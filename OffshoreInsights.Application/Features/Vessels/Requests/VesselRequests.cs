using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Vessels.Requests;

/// <summary>
/// Retrieves a single vessel by its MMSI number.
/// </summary>
public record GetVesselByMmsiRequest(long Mmsi);

/// <summary>
/// Retrieves a filtered, paginated list of vessels.
/// Optionally accepts a list of MMSIs for a targeted batch lookup, or type and
/// wind relevance filters for broader queries.
/// </summary>
public record GetVesselsRequest
{
    public IEnumerable<long>? Mmsis { get; init; }
    public VesselType? Type { get; init; }
    public bool? IsOffshoreWindRelevant { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

/// <summary>
/// Retrieves the historic position track for a vessel over a requested time range.
/// Provide a <see cref="Period"/> for a preset window, or supply <see cref="From"/> and/or
/// <see cref="To"/> for a custom range. At least one of the three must be present.
/// Custom <see cref="From"/>/<see cref="To"/> values take precedence over <see cref="Period"/>.
/// </summary>
public record GetVesselTrackByMmsiRequest
{
    public long Mmsi { get; init; }
    public TrackPeriod? Period { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

/// <summary>
/// Retrieves the current position for a single vessel by its MMSI number.
/// </summary>
public record GetVesselPositionByMmsiRequest(long Mmsi);

/// <summary>
/// Retrieves current positions for a collection of vessels.
/// Optionally accepts a list of MMSIs for a targeted batch lookup, or a geographic
/// bounding box to return all vessels currently within that area.
/// </summary>
public record GetVesselPositionsRequest
{
    public IEnumerable<long>? Mmsis { get; init; }
    public double? MinLatitude { get; init; }
    public double? MaxLatitude { get; init; }
    public double? MinLongitude { get; init; }
    public double? MaxLongitude { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
