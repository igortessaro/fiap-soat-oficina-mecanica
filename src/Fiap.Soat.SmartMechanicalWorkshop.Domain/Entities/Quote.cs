using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class Quote : Entity
{
    private Quote() { }

    public Guid ServiceOrderId { get; private set; }
    public QuoteStatus Status { get; private set; } = QuoteStatus.Pending;
    public decimal Total { get; private set; }
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public IReadOnlyList<QuoteService> Services { get; private set; } = [];
    public IReadOnlyList<QuoteSupply> Supplies { get; private set; } = [];
}
