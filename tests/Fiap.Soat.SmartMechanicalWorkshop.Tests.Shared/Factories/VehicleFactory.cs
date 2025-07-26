using Bogus;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

[ExcludeFromCodeCoverage]
public static class VehicleFactory
{
    public static List<Vehicle> VehiclesList { get; } = new Faker<Vehicle>("pt_BR")
        .RuleFor(v => v.Id, f => f.Random.Guid())
        .RuleFor(v => v.LicensePlate, f => (LicensePlate) f.Random.Replace("???#?##").ToUpper())
        .RuleFor(v => v.Model, f => f.Vehicle.Model())
        .RuleFor(v => v.ManufactureYear, f => f.Date.Past(10).Year)
        .RuleFor(v => v.Brand, f => f.Vehicle.Manufacturer())
        .Generate(10);

    public static Vehicle CreateVehicle(bool validLicensePlate = true)
    {
        var faker = new Faker("pt_BR");
        return new Vehicle(
            faker.Vehicle.Model(),
            faker.Vehicle.Manufacturer(),
            DateTime.Now.Year,
            validLicensePlate ? (LicensePlate) CreateValidLicensePlate() : CreateInvalidLicensePlate(),
            faker.Random.Guid());
    }


    public static string CreateValidLicensePlate() => new Faker("pt_BR").Random.Replace("???#?##").ToUpper();
    public static string CreateInvalidLicensePlate() => new Faker().Random.Replace("##??###");
}
