using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

public record CreateNewSupplyRequest
{
    [Required] public string Name { get; init; } = string.Empty;

    [Required] public int Quantity { get; init; }

    [Required] public decimal Price { get; init; }
}
