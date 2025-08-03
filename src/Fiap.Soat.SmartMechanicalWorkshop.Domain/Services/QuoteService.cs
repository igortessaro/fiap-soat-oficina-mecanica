using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class QuoteService(IMapper mapper, IQuoteRepository quoteRepository) : IQuoteService
{
    public async Task<Response<QuoteDto>> CreateAsync(ServiceOrderDto serviceOrder, CancellationToken cancellationToken)
    {
        if (serviceOrder.Status != ServiceOrderStatus.WaitingApproval)
        {
            return ResponseFactory.Fail<QuoteDto>($"Service order must be in {nameof(ServiceOrderStatus.WaitingApproval)} status to create a quote");
        }

        if (!serviceOrder.AvailableServices.Any())
        {
            return ResponseFactory.Fail<QuoteDto>("Service order does not have available services");
        }

        var quote = new Quote(serviceOrder.Id);
        serviceOrder.AvailableServices.ToList().ForEach(availableService => quote.AddService(availableService.Id, availableService.Price));
        serviceOrder.AvailableServices.SelectMany(x => x.Supplies).ToList().ForEach(supply => quote.AddSupply(supply.Id, supply.Price, supply.Quantity));
        var entity = await quoteRepository.AddAsync(quote, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<QuoteDto>(entity));
    }

    public async Task<Response<QuoteDto>> PatchAsync(Guid id, QuoteStatus status, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(id, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<QuoteDto>("Quote not found", HttpStatusCode.NotFound);
        }

        if (status == quote.Status)
        {
            return ResponseFactory.Fail<QuoteDto>($"Quote is already {status.ToString()}");
        }

        if (quote.Status != QuoteStatus.Pending)
        {
            return ResponseFactory.Fail<QuoteDto>($"Quote is not in {nameof(QuoteStatus.Pending)} status");
        }

        switch (status)
        {
            case QuoteStatus.Approved:
                _ = quote.Approve();
                break;
            case QuoteStatus.Rejected:
                _ = quote.Reject();
                break;
            case QuoteStatus.Pending:
            default:
                return ResponseFactory.Fail<QuoteDto>($"Invalid status {status}");
        }

        var quoteUpdated = await quoteRepository.UpdateAsync(quote, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<QuoteDto>(quoteUpdated));
    }
}
