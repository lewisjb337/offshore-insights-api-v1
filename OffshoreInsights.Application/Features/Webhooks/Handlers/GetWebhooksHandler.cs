using MediatR;
using OffshoreInsights.Application.Features.Webhooks.Abstractions;
using OffshoreInsights.Application.Features.Webhooks.Queries;
using OffshoreInsights.Application.Features.Webhooks.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Webhooks.Handlers;

public class GetWebhooksHandler(IWebhooksData webhooksData) : IRequestHandler<GetWebhooksQuery, PagedResponse<WebhookResponse>>
{
    public async Task<PagedResponse<WebhookResponse>> Handle(GetWebhooksQuery query, CancellationToken cancellationToken)
    {
        var results = await webhooksData.GetWebhooksAsync(query.Request, cancellationToken);

        return new PagedResponse<WebhookResponse>
        {
            Items      = results.Items.Select(w => (WebhookResponse)w),
            Page       = results.Page,
            PageSize   = results.PageSize,
            TotalCount = results.TotalCount,
            TotalPages = results.TotalPages
        };
    }
}
