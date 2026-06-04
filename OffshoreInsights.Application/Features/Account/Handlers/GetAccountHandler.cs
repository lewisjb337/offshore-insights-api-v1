using MediatR;
using OffshoreInsights.Application.Features.Account.Queries;
using OffshoreInsights.Application.Features.Account.Responses;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Account.Handlers;

/// <summary>
/// Returns account plan and usage information for the authenticated caller.
/// TODO: Replace placeholder values with real data once auth and usage tracking are wired up.
/// </summary>
public class GetAccountHandler : IRequestHandler<GetAccountQuery, AccountResponse>
{
    public Task<AccountResponse> Handle(GetAccountQuery query, CancellationToken cancellationToken)
    {
        // Placeholder — will be replaced with real per-user plan/usage data from auth context.
        var response = new AccountResponse
        {
            Plan             = AccountPlan.Starter,
            CallsThisPeriod  = 0,
            CallLimit        = 1_000,
            PeriodResetsAt   = new DateTimeOffset(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, TimeSpan.Zero).AddMonths(1),
            IsRateLimited    = false
        };

        return Task.FromResult(response);
    }
}
