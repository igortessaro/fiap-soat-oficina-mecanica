using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;

public sealed class UpdateServiceOrderStatusHandler(IMapper mapper, IMediator mediator, IServiceOrderRepository serviceOrderRepository)
    : IRequestHandler<UpdateServiceOrderStatusCommand, Response<ServiceOrder>>, INotificationHandler<UpdateQuoteStatusNotification>
{
    public async Task<Response<ServiceOrder>> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await serviceOrderRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<ServiceOrder>("Service Order not found", HttpStatusCode.NotFound);
        }

        _ = entity.ChangeStatus(request.Status);
        _ = await serviceOrderRepository.UpdateAsync(entity, cancellationToken);
        var response = (await serviceOrderRepository.GetDetailedAsync(request.Id, cancellationToken))!;
        await mediator.Publish(new UpdateServiceOrderStatusNotification(request.Id, response), cancellationToken);
        return ResponseFactory.Ok(response);
    }

    public Task Handle(UpdateQuoteStatusNotification notification, CancellationToken cancellationToken) =>
        mediator.Send(new UpdateServiceOrderStatusCommand(notification.Quote.ServiceOrderId, ServiceOrder.GetNextStatus(notification.Quote.Status)), cancellationToken);
}
