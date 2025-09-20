namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class QuoteAvailableService
{
    private QuoteAvailableService() { }

    public QuoteAvailableService(Guid quoteId, Guid serviceId, decimal price)
    {
        QuoteId = quoteId;
        ServiceId = serviceId;
        Price = price;
    }

    public Guid Id { get; private set; }
    public Guid QuoteId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal Price { get; private set; }
    public Quote Quote { get; private set; } = null!;
    public AvailableService Service { get; private set; } = null!;
}
