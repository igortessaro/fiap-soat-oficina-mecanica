using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class QuoteService(ILogger<QuoteService> logger, IQuoteRepository quoteRepository) : IQuoteService
{
    public async Task<Response<Quote>> CreateAsync(ServiceOrderDto serviceOrder, CancellationToken cancellationToken)
    {
        if (serviceOrder.Status != EServiceOrderStatus.WaitingApproval)
        {
            logger.LogInformation("Service order with Id {ServiceOrderId} is not in WaitingApproval status, no quote will be created", serviceOrder.Id);
            return ResponseFactory.Fail<Quote>(new FluentResults.Error("Is only allowed to quote waiting approval service orders"), System.Net.HttpStatusCode.BadRequest);
        }

        if (!serviceOrder.AvailableServices.Any())
        {
            return ResponseFactory.Fail<Quote>(new FluentResults.Error("Service order does not have available services"), System.Net.HttpStatusCode.BadRequest);
        }

        var quote = new Quote(serviceOrder.Id);
        serviceOrder.AvailableServices.ToList().ForEach(availableService => quote.AddService(availableService.Id, availableService.Price));
        serviceOrder.AvailableServices.SelectMany(x => x.Supplies).ToList().ForEach(supply => quote.AddSupply(supply.Id, supply.Price, supply.Quantity));
        var entity = await quoteRepository.AddAsync(quote, cancellationToken);
        return ResponseFactory.Ok<Quote>(entity);
    }
}
