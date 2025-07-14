using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record UpdateOneServiceOrderInput
{
    public Guid Id { get; init; }
    public IReadOnlyList<Guid>? ServiceIds { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public EServiceOrderStatus? ServiceOrderStatus { get; init; }

    public UpdateOneServiceOrderInput() { }
    public UpdateOneServiceOrderInput(
        Guid id,
        IReadOnlyList<Guid>? serviceIds,
        string? title,
        string? description,
        EServiceOrderStatus? serviceOrderStatus)
    {
        Id = id;
        ServiceIds = serviceIds;
        Title = title;
        Description = description;
        ServiceOrderStatus = serviceOrderStatus;
    }

    public UpdateOneServiceOrderInput(Guid id, EServiceOrderStatus status)
    {
        Id = id;
        ServiceOrderStatus = status;
    }

    public UpdateOneServiceOrderInput(Guid id)
    {
        Id = id;
    }
}
