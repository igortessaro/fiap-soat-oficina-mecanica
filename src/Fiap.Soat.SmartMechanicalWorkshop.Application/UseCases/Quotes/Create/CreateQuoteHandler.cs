using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Create;

public sealed class CreateQuoteHandler(IQuoteRepository quoteRepository) : INotificationHandler<UpdateServiceOrderStatusNotification>
{
    public async Task Handle(UpdateServiceOrderStatusNotification notification, CancellationToken cancellationToken)
    {
        var serviceOrder = notification.ServiceOrder;
        if (serviceOrder.Status != ServiceOrderStatus.WaitingApproval) return;
        if (!serviceOrder.AvailableServices.Any()) return;

        var quote = new Quote(serviceOrder.Id);
        serviceOrder.AvailableServices.ToList().ForEach(availableService => quote.AddService(availableService.Id, availableService.Price));
        serviceOrder.AvailableServices.SelectMany(x => x.AvailableServiceSupplies).ToList().ForEach(availableServiceSupply => quote.AddSupply(availableServiceSupply.SupplyId, availableServiceSupply.Supply.Price, availableServiceSupply.Supply.Quantity));
        _ = await quoteRepository.AddAsync(quote, cancellationToken);
    }
}
