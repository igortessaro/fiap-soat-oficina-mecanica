namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public record Vehicle : Entity
{
    public string LicensePlate { get; private set; } = string.Empty;
    public int ManufactureYear { get; private set; }
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public Guid ClientId { get; private set; }

    public Client Client { get; private set; } = null!;

    public Vehicle Update(int? manufactureYear, string licensePlate, string brand, string model)
    {
        if (manufactureYear.HasValue) ManufactureYear = manufactureYear.Value;
        if (!string.IsNullOrEmpty(licensePlate)) LicensePlate = licensePlate;
        if (!string.IsNullOrEmpty(brand)) Brand = brand;
        if (!string.IsNullOrEmpty(model)) Model = model;
        return this;
    }
}
