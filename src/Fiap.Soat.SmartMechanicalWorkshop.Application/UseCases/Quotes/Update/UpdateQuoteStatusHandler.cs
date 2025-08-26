using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;

public sealed class UpdateQuoteStatusHandler(IMediator mediator, IQuoteRepository quoteRepository) : IRequestHandler<UpdateQuoteStatusCommand, Response<Quote>>
{
    public async Task<Response<Quote>> Handle(UpdateQuoteStatusCommand request, CancellationToken cancellationToken)
    {
        var quote = await quoteRepository.GetByIdAsync(request.Id, cancellationToken);
        if (quote is null)
        {
            return ResponseFactory.Fail<Quote>("Quote not found", HttpStatusCode.NotFound);
        }

        if (request.Status == quote.Status)
        {
            return ResponseFactory.Fail<Quote>($"Quote is already {request.Status.ToString()}");
        }

        if (quote.Status != QuoteStatus.Pending)
        {
            return ResponseFactory.Fail<Quote>($"Quote is not in {nameof(QuoteStatus.Pending)} status");
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
                return ResponseFactory.Fail<Quote>($"Invalid status {request.Status}");
        }

        var quoteUpdated = await quoteRepository.UpdateAsync(quote, cancellationToken);
        await mediator.Publish(new UpdateQuoteStatusNotification(request.Id, quoteUpdated), cancellationToken);
        return ResponseFactory.Ok(quoteUpdated);
    }
}
