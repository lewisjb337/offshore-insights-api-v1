using MediatR;
using OffshoreInsights.Application.Features.Account.Abstractions;
using OffshoreInsights.Application.Features.Account.Queries;
using OffshoreInsights.Application.Features.Account.Responses;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Account.Handlers;

public class GetAccountHandler(IAccountRepository repository) : IRequestHandler<GetAccountQuery, AccountResponse>
{
    private static readonly Dictionary<AccountPlan, long?> CallLimits = new()
    {
        [AccountPlan.Free]         = 100,
        [AccountPlan.Starter]      = 5_000,
        [AccountPlan.Professional] = 50_000,
        [AccountPlan.Enterprise]   = null, // unlimited
    };

    public async Task<AccountResponse> Handle(GetAccountQuery query, CancellationToken cancellationToken)
    {
        var summary = await repository.GetSummaryAsync(query.UserId, cancellationToken);
        var limit   = CallLimits.GetValueOrDefault(summary.Plan, 100);

        return new AccountResponse
        {
            Plan            = summary.Plan,
            Status          = summary.Status,
            CallsThisPeriod = summary.CallsThisPeriod,
            CallLimit       = limit,
            PeriodStart     = summary.PeriodStart,
            PeriodResetsAt  = summary.PeriodEnd,
            IsRateLimited   = limit.HasValue && summary.CallsThisPeriod >= limit.Value,
        };
    }
}
