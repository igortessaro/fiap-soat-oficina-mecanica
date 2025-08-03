namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class QuoteSupply
{
    private QuoteSupply() { }

    public QuoteSupply(Guid quoteId, Guid supplyId, decimal price, int quantity)
    {
        QuoteId = quoteId;
        SupplyId = supplyId;
        Price = price;
        Quantity = quantity;
    }

    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid SupplyId { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public Quote Quote { get; private set; } = null!;
    public Supply Supply { get; private set; } = null!;
}
