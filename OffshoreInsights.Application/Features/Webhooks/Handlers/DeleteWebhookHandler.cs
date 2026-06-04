using MediatR;
using OffshoreInsights.Application.Features.Webhooks.Abstractions;
using OffshoreInsights.Application.Features.Webhooks.Queries;

namespace OffshoreInsights.Application.Features.Webhooks.Handlers;

public class DeleteWebhookHandler(IWebhooksData webhooksData) : IRequestHandler<DeleteWebhookCommand>
{
    public async Task Handle(DeleteWebhookCommand command, CancellationToken cancellationToken)
    {
        await webhooksData.DeleteWebhookAsync(command.Request, cancellationToken);
    }
}
