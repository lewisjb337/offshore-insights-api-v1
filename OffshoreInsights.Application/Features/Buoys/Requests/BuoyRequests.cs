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
/// Retrieves the latest position and telemetry for a single buoy by its ID.
/// </summary>
public record GetBuoyPositionByIdRequest(long Id);

/// <summary>
/// Retrieves a historic position and telemetry track for a buoy over a requested time range.
/// Provide a <see cref="Period"/> for a preset window, or supply <see cref="From"/> and/or
/// <see cref="To"/> for a custom range. At least one of the three must be present.
/// Custom <see cref="From"/>/<see cref="To"/> values take precedence over <see cref="Period"/>.
/// </summary>
public record GetBuoyTrackByIdRequest
{
    public long Id { get; init; }
    public TrackPeriod? Period { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
