using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record ServiceSupplyRequest(Guid SupplyId, int Quantity);
