namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record ServiceSupplyDto(Guid AvailableServiceId, Guid SupplyId, int Quantity);
