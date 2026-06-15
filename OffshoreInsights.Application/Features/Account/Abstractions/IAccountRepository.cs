using OffshoreInsights.Application.Features.Account.Models;

namespace OffshoreInsights.Application.Features.Account.Abstractions;

public interface IAccountRepository
{
    Task<AccountSummary> GetSummaryAsync(string userId, CancellationToken cancellationToken = default);
}