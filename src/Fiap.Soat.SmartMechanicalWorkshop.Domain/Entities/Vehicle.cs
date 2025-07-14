namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class Vehicle : Entity
{
    private Vehicle() { }

    public Vehicle(string model, string brand, int manufactureYear, string licensePlate, Guid personId) : this()
    {
        Model = model;
        Brand = brand;
        ManufactureYear = manufactureYear;
        LicensePlate = licensePlate.Trim().ToUpper();
        PersonId = personId;
    }

    public string LicensePlate { get; private set; } = string.Empty;
    public int ManufactureYear { get; private set; }
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public Guid PersonId { get; private set; }

    public Person Person { get; private set; } = null!;

    public Vehicle Update(int? manufactureYear, string licensePlate, string brand, string model)
    {
        if (manufactureYear.HasValue) ManufactureYear = manufactureYear.Value;
        if (!string.IsNullOrEmpty(licensePlate)) LicensePlate = licensePlate.Trim().ToUpper();
        if (!string.IsNullOrEmpty(brand)) Brand = brand;
        if (!string.IsNullOrEmpty(model)) Model = model;
        return this;
    }
}
