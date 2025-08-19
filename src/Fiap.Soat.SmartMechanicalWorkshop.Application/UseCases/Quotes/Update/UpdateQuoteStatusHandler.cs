using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;

public sealed class UpdateQuoteStatusHandler(IMediator mediator, IQuoteService service) : IRequestHandler<UpdateQuoteStatusCommand, Response<QuoteDto>>
{
    public async Task<Response<QuoteDto>> Handle(UpdateQuoteStatusCommand request, CancellationToken cancellationToken)
    {
        var response = await service.PatchAsync(request.Id, request.Status, cancellationToken);
        if (response.IsSuccess)
        {
            await mediator.Publish(new UpdateQuoteStatusNotification(request.Id, response.Data), cancellationToken);
        }

        return response;
    }
}
