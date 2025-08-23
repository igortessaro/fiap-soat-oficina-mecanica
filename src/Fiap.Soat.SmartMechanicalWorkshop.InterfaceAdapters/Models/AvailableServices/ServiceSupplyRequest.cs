using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record ServiceSupplyRequest(Guid SupplyId, int Quantity);
