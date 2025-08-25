using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;

public sealed class UpdateQuoteStatusHandler(IMapper mapper, IMediator mediator, IQuoteRepository quoteRepository) : IRequestHandler<UpdateQuoteStatusCommand, Response<QuoteDto>>
{
    public async Task<Response<QuoteDto>> Handle(UpdateQuoteStatusCommand request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(request.Id, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<QuoteDto>("Quote not found", HttpStatusCode.NotFound);
        }

        if (request.Status == quote.Status)
        {
            return ResponseFactory.Fail<QuoteDto>($"Quote is already {request.Status.ToString()}");
        }

        if (quote.Status != QuoteStatus.Pending)
        {
            return ResponseFactory.Fail<QuoteDto>($"Quote is not in {nameof(QuoteStatus.Pending)} status");
        }

        switch (request.Status)
        {
            case QuoteStatus.Approved:
                _ = quote.Approve();
                break;
            case QuoteStatus.Rejected:
                _ = quote.Reject();
                break;
            case QuoteStatus.Pending:
            default:
                return ResponseFactory.Fail<QuoteDto>($"Invalid status {request.Status}");
        }

        var quoteUpdated = await quoteRepository.UpdateAsync(quote, cancellationToken);
        var response = mapper.Map<QuoteDto>(quoteUpdated);
        await mediator.Publish(new UpdateQuoteStatusNotification(request.Id, response), cancellationToken);
        return ResponseFactory.Ok(response);
    }
}
