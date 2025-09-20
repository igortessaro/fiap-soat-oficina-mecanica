using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Stock;

public sealed class ReplacementStockHandler(IMediator mediator, IQuoteRepository quoteRepository) : INotificationHandler<UpdateQuoteStatusNotification>
{
    public async Task Handle(UpdateQuoteStatusNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Quote.Status != QuoteStatus.Approved) return;

        var quote = await quoteRepository.GetDetailedByIdAsync(notification.Quote.Id, cancellationToken);
        if (quote is null || !quote.Supplies.Any()) return;

        foreach (var supply in quote.Supplies)
        {
            var supplyResponse = await mediator.Send(new UpdateStockCommand(supply.SupplyId, supply.Quantity, false), cancellationToken);

            if (supplyResponse is { IsSuccess: true, Data.Quantity: <= 0 })
            {
                _ = await mediator.Send(new UpdateStockCommand(supply.SupplyId, 100 - supplyResponse.Data.Quantity, true), cancellationToken);
            }
        }
    }
}
