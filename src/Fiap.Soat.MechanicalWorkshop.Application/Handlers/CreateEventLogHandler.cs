using Fiap.Soat.MechanicalWorkshop.Application.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class CreateEventLogHandler(ILogger<CreateEventLogHandler> logger) : INotificationHandler<ServiceOrderChangeStatusNotification>
{
    public Task Handle(ServiceOrderChangeStatusNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {@ServiceOrderChangeStatusNotification}", notification);
        return Task.CompletedTask;
    }
}
