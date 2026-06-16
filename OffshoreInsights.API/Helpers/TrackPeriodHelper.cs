using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.API.Helpers;

internal static class TrackPeriodHelper
{
    /// <summary>
    /// Converts a <see cref="TrackPeriod"/> preset into an absolute UTC <c>from</c> value
    /// relative to the current time.
    /// </summary>
    internal static DateTimeOffset ToFromUtc(TrackPeriod period) => period switch
    {
        TrackPeriod.Last1Hour   => DateTimeOffset.UtcNow.AddHours(-1),
        TrackPeriod.Last6Hours  => DateTimeOffset.UtcNow.AddHours(-6),
        TrackPeriod.Last24Hours => DateTimeOffset.UtcNow.AddHours(-24),
        TrackPeriod.Last7Days   => DateTimeOffset.UtcNow.AddDays(-7),
        TrackPeriod.Last30Days  => DateTimeOffset.UtcNow.AddDays(-30),
        _                       => DateTimeOffset.UtcNow.AddHours(-24)
    };
}
