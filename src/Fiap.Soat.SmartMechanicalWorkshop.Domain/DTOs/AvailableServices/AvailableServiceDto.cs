namespace Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.AvailableServices
{
        public record AvailableServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}