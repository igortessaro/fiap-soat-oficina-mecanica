using Fiap.Soat.MechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class CreateEventLogHandler(IServiceOrderEventService service) : INotificationHandler<ServiceOrderChangeStatusNotification>
{
    public Task Handle(ServiceOrderChangeStatusNotification notification, CancellationToken cancellationToken) =>
        service.CreateAsync(notification.Id, notification.ServiceOrder.Status, cancellationToken);
}
