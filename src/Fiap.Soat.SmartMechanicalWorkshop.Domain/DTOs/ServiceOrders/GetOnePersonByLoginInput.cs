namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
public record GetOnePersonByLoginInput(Guid id, string email)
{
    public string Email { get; set; } = email;
    public Guid Id { get; set; } = id;
}
