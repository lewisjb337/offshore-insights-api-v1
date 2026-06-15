using Microsoft.EntityFrameworkCore;
using OffshoreInsights.Application.Features.Account.Abstractions;
using OffshoreInsights.Application.Features.Account.Models;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Repositories;

public class AccountData(ApplicationDbContext context) : IAccountRepository
{
    public async Task<AccountSummary> GetSummaryAsync(string userId, CancellationToken cancellationToken = default)
    {
        var sub = await context.AccountSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        // Pending = checkout abandoned, no confirmed subscription — same logic as frontend
        var planStr = (sub is null || sub.Status == "Pending") ? "Free" : sub.Plan;
        var plan    = Enum.TryParse<AccountPlan>(planStr, out var p) ? p : AccountPlan.Free;
        var status  = (sub is null || sub.Status == "Pending") ? "Active" : sub.Status;

        // Billing period: use Stripe's period if available, otherwise calendar month
        var now            = DateTimeOffset.UtcNow;
        var periodStart    = sub?.CurrentPeriodStart ?? new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var periodEnd      = sub?.CurrentPeriodEnd   ?? periodStart.AddMonths(1);

        var periodStartDate = new DateOnly(periodStart.Year, periodStart.Month, 1);

        var usage = await context.ApiCallUsage
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId && u.PeriodStart == periodStartDate, cancellationToken);

        return new AccountSummary(plan, status, usage?.CallCount ?? 0, periodStart, periodEnd);
    }
}
