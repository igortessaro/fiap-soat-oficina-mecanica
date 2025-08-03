using Fiap.Soat.MechanicalWorkshop.Application.Commands;
using Fiap.Soat.MechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class QuoteChangeStatusHandler(IMediator mediator, IQuoteService service) : IRequestHandler<QuoteChangeStatusCommand, Response<QuoteDto>>
{
    public async Task<Response<QuoteDto>> Handle(QuoteChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var response = await service.PatchAsync(request.Id, request.Status, cancellationToken);
        if (response.IsSuccess)
        {
            await mediator.Publish(new QuoteChangeStatusNotification(request.Id, response.Data), cancellationToken);
        }

        return response;
    }
}
