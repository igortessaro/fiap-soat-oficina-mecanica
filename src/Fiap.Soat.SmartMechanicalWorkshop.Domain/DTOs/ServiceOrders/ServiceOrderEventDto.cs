using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record ServiceOrderEventDto(Guid Id, ServiceOrderStatus Status, DateTime CreatedAt);
