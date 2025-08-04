using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record ServiceSupplyRequest(Guid SupplyId, int Quantity);
