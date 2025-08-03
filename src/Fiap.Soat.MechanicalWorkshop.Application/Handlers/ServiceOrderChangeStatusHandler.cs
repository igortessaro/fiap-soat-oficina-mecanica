using Fiap.Soat.MechanicalWorkshop.Application.Commands;
using Fiap.Soat.MechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class ServiceOrderChangeStatusHandler(IServiceOrderService service, IMediator mediator)
    : IRequestHandler<ServiceOrderChangeStatusCommand, Response<ServiceOrderDto>>, INotificationHandler<QuoteChangeStatusNotification>
{
    public async Task<Response<ServiceOrderDto>> Handle(ServiceOrderChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var response = await service.PatchAsync(new PatchOneServiceOrderInput(request.Id, request.Status), cancellationToken);
        if (response.IsSuccess)
        {
            await mediator.Publish(new ServiceOrderChangeStatusNotification(request.Id, response.Data), cancellationToken);
        }

        return response;
    }

    public Task Handle(QuoteChangeStatusNotification notification, CancellationToken cancellationToken) =>
        mediator.Send(new ServiceOrderChangeStatusCommand(notification.Quote.ServiceOrderId, ServiceOrder.GetNextStatus(notification.Quote.Status)), cancellationToken);
}
