using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Account.Responses;

public class AccountResponse
{
    /// <summary>The account's current subscription plan.</summary>
    public AccountPlan Plan { get; set; }

    /// <summary>Number of API calls made during the current billing period.</summary>
    public long CallsThisPeriod { get; set; }

    /// <summary>Maximum API calls allowed in the current billing period. Null means unlimited.</summary>
    public long? CallLimit { get; set; }

    /// <summary>UTC date and time when the current billing period resets.</summary>
    public DateTimeOffset PeriodResetsAt { get; set; }

    /// <summary>Whether the account is currently rate-limited.</summary>
    public bool IsRateLimited { get; set; }
}
