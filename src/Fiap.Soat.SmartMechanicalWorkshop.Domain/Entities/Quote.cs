using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class Quote : Entity
{
    private Quote() { }

    public Quote(Guid serviceOrderId)
        : this()
    {
        ServiceOrderId = serviceOrderId;
    }

    public Guid ServiceOrderId { get; private set; }
    public QuoteStatus Status { get; private set; } = QuoteStatus.Pending;
    public decimal Total { get; private set; }
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public ICollection<QuoteAvailableService> Services { get; private set; } = [];
    public ICollection<QuoteSupply> Supplies { get; private set; } = [];

    public Quote AddService(Guid availableServiceId, decimal price)
    {
        Services.Add(new QuoteAvailableService(Id, availableServiceId, price));
        Total += price;
        return this;
    }

    public Quote AddSupply(Guid supplyId, decimal price, int quantity)
    {
        Supplies.Add(new QuoteSupply(Id, supplyId, price, quantity));
        Total += price * quantity;
        return this;
    }

    public Quote Approve()
    {
        Status = QuoteStatus.Approved;
        return this;
    }

    public Quote Reject()
    {
        Status = QuoteStatus.Rejected;
        return this;
    }
}
