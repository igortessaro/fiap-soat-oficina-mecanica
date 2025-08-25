using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;

public record UpdateQuoteStatusCommand(Guid Id, QuoteStatus Status, Guid ServiceOrderId) : IRequest<Response<QuoteDto>>;
