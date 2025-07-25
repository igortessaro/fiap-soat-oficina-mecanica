using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Notifications;

public record ServiceOrderChangeStatusNotification(Guid Id, ServiceOrderDto ServiceOrder) : INotification;
