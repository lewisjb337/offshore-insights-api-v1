using OffshoreInsights.Application.Features.Webhooks.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Webhooks.Abstractions;

public interface IWebhooksData
{
    Task<PagedResponse<Webhook>> GetWebhooksAsync(GetWebhooksRequest request, CancellationToken cancellationToken = default);
    Task<Webhook> RegisterWebhookAsync(RegisterWebhookRequest request, CancellationToken cancellationToken = default);
    Task DeleteWebhookAsync(DeleteWebhookRequest request, CancellationToken cancellationToken = default);
}
