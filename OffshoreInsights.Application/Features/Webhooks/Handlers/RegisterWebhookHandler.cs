using MediatR;
using OffshoreInsights.Application.Features.Webhooks.Abstractions;
using OffshoreInsights.Application.Features.Webhooks.Queries;
using OffshoreInsights.Application.Features.Webhooks.Responses;

namespace OffshoreInsights.Application.Features.Webhooks.Handlers;

public class RegisterWebhookHandler(IWebhooksData webhooksData) : IRequestHandler<RegisterWebhookCommand, WebhookResponse>
{
    public async Task<WebhookResponse> Handle(RegisterWebhookCommand command, CancellationToken cancellationToken)
    {
        var result = await webhooksData.RegisterWebhookAsync(command.Request, cancellationToken);

        return result;
    }
}
