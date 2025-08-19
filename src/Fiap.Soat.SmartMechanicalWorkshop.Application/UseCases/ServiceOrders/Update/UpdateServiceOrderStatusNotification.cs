using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;

public record UpdateServiceOrderStatusNotification(Guid Id, ServiceOrderDto ServiceOrder) : INotification;
