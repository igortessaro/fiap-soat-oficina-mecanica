using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record ServiceSupplyDto(Guid AvailableServiceId, Guid SupplyId, int Quantity);
