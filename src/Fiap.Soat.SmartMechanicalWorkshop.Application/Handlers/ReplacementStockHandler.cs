using Fiap.Soat.SmartMechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Handlers;

public sealed class ReplacementStockHandler(IQuoteRepository quoteRepository, ISupplyService supplyService) : INotificationHandler<QuoteChangeStatusNotification>
{
    public async Task Handle(QuoteChangeStatusNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Quote.Status != QuoteStatus.Approved) return;

        var quote = await quoteRepository.GetDetailedByIdAsync(notification.Quote.Id, cancellationToken);
        if (quote is null || !quote.Supplies.Any()) return;

        foreach (var supply in quote.Supplies)
        {
            var supplyResponse = await supplyService.ChangeStock(supply.SupplyId, supply.Quantity, false, cancellationToken);

            if (supplyResponse is { IsSuccess: true, Data.Quantity: <= 0 })
            {
                await supplyService.ChangeStock(supply.SupplyId, 100 - supplyResponse.Data.Quantity, true, cancellationToken);
            }
        }
    }
}
