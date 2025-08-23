using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record CreateAvailableServiceRequest(
    [Required][MaxLength(100)] string Name,
    [Required] decimal Price,
    IReadOnlyList<ServiceSupplyRequest> Supplies);
