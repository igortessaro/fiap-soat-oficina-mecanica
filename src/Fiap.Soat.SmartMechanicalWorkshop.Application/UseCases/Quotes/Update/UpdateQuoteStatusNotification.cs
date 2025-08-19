using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;

public record UpdateQuoteStatusNotification(Guid Id, QuoteDto Quote) : INotification;
