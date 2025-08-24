using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.EventLog;

public sealed class CreateEventLogHandler(IServiceOrderEventRepository repository) : INotificationHandler<UpdateServiceOrderStatusNotification>
{
    public Task Handle(UpdateServiceOrderStatusNotification notification, CancellationToken cancellationToken) =>
        repository.AddAsync(new ServiceOrderEvent(notification.Id, notification.ServiceOrder.Status), cancellationToken);
}
