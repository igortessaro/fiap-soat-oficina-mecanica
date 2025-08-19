using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record ServiceSupplyRequest(Guid SupplyId, int Quantity);
