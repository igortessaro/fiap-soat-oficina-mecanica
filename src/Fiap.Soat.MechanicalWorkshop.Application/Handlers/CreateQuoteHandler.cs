using Fiap.Soat.MechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class CreateQuoteHandler(IQuoteService service) : INotificationHandler<ServiceOrderChangeStatusNotification>
{
    public Task Handle(ServiceOrderChangeStatusNotification notification, CancellationToken cancellationToken) =>
        service.CreateAsync(notification.ServiceOrder, cancellationToken);
}
