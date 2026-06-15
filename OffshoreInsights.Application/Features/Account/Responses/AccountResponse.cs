using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Account.Responses;

public class AccountResponse
{
    public AccountPlan Plan { get; set; }
    public string Status { get; set; } = "Active";
    public long CallsThisPeriod { get; set; }

    /// <summary>Null means unlimited (Enterprise plan).</summary>
    public long? CallLimit { get; set; }

    public DateTimeOffset PeriodStart { get; set; }
    public DateTimeOffset PeriodResetsAt { get; set; }
    public bool IsRateLimited { get; set; }
}
