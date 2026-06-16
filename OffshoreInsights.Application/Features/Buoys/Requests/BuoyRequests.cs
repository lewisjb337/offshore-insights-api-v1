using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Buoys.Requests;

/// <summary>
/// Retrieves a paginated list of all buoys.
/// </summary>
public record GetBuoysRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

/// <summary>
/// Retrieves the latest position for a single buoy by its MMSI.
/// </summary>
public record GetBuoyPositionByIdRequest(long Mmsi);

/// <summary>
/// Retrieves a historic position track for a buoy over a requested time range.
/// </summary>
public record GetBuoyTrackByIdRequest
{
    public long Mmsi { get; init; }
    public TrackPeriod? Period { get; init; }
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
