using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Notifications;

public record QuoteChangeStatusNotification(Guid Id, QuoteDto Quote) : INotification;
