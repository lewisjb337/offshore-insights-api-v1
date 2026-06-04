using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.API.Helpers;

internal static class TrackPeriodHelper
{
    /// <summary>
    /// Converts a <see cref="TrackPeriod"/> preset into an absolute UTC <c>from</c> value
    /// relative to the current time.
    /// </summary>
    internal static DateTime ToFromUtc(TrackPeriod period) => period switch
    {
        TrackPeriod.Last1Hour   => DateTime.UtcNow.AddHours(-1),
        TrackPeriod.Last6Hours  => DateTime.UtcNow.AddHours(-6),
        TrackPeriod.Last24Hours => DateTime.UtcNow.AddHours(-24),
        TrackPeriod.Last7Days   => DateTime.UtcNow.AddDays(-7),
        TrackPeriod.Last30Days  => DateTime.UtcNow.AddDays(-30),
        _                       => DateTime.UtcNow.AddHours(-24)
    };
}
