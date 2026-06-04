namespace OffshoreInsights.Domain.Enums;

/// <summary>
/// Preset time window for track history endpoints.
/// Selecting a period automatically calculates <c>from</c> and <c>to</c> relative to the
/// current UTC time, so callers do not need to supply raw datetime values.
/// Custom <c>from</c>/<c>to</c> query parameters override this when provided.
/// </summary>
public enum TrackPeriod
{
    Last1Hour,
    Last6Hours,
    Last24Hours,
    Last7Days,
    Last30Days
}
