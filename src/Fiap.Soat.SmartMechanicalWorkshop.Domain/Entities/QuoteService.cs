namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class QuoteService
{
    private QuoteService() { }

    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal Price { get; private set; }
    public Quote Quote { get; private set; } = null!;
    public AvailableService Service { get; private set; } = null!;

    public QuoteService(Guid quoteId, Guid serviceId, decimal price)
    {
        QuoteId = quoteId;
        ServiceId = serviceId;
        Price = price;
    }
}
