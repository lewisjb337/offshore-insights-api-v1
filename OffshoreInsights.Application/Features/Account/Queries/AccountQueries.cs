using MediatR;
using OffshoreInsights.Application.Features.Account.Responses;

namespace OffshoreInsights.Application.Features.Account.Queries;

public record GetAccountQuery(string UserId) : IRequest<AccountResponse>;
