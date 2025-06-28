namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public record Entity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
}
