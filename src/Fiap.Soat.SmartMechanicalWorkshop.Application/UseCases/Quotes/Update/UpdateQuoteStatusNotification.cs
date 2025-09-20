using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Quotes.Update;

public record UpdateQuoteStatusNotification(Guid Id, Quote Quote) : INotification;
