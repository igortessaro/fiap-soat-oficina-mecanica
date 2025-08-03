using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record PatchOneServiceOrderInput(Guid Id, ServiceOrderStatus Status);
