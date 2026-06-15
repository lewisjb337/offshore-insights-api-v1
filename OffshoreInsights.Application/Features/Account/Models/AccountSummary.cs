using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Account.Models;

public record AccountSummary(
    AccountPlan Plan,
    string Status,
    long CallsThisPeriod,
    DateTimeOffset PeriodStart,
    DateTimeOffset PeriodEnd
);
