namespace OffshoreInsights.Domain.Entities;

/// <summary>
/// Tracks the number of API calls made by a user within a billing period.
/// One row per (UserId, PeriodStart). The period always starts on the 1st of the month.
/// </summary>
public class ApiCallUsage
{
    public long Id { get; set; }

    /// <summary>Foreign key — matches ApiKeys.UserId (auth.users uuid stored as string).</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>First day of the billing month, e.g. 2025-06-01.</summary>
    public DateOnly PeriodStart { get; set; }

    public long CallCount { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
