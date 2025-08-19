using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class QuoteService(IMapper mapper, IQuoteRepository quoteRepository) : IQuoteService
{
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
