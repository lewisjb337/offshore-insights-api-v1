using MediatR;
using OffshoreInsights.Application.Features.Webhooks.Requests;
using OffshoreInsights.Application.Features.Webhooks.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Webhooks.Queries;

public record GetWebhooksQuery(GetWebhooksRequest Request) : IRequest<PagedResponse<WebhookResponse>>;
public record RegisterWebhookCommand(RegisterWebhookRequest Request) : IRequest<WebhookResponse>;
public record DeleteWebhookCommand(DeleteWebhookRequest Request) : IRequest;
