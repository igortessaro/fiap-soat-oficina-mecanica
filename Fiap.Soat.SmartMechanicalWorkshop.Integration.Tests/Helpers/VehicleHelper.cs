using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Helpers;

public static class VehicleHelper
{
    private static readonly List<Vehicle> Vehicles =
    [
        CreateVehicle("Civic", "Honda", 2020, "ABC1234"),
        CreateVehicle("Corolla", "Toyota", 2019, "XYZ5678"),
        CreateVehicle("Gol", "Volkswagen", 2018, "DEF4321"),
        CreateVehicle("Fiesta", "Ford", 2021, "GHI8765"),
        CreateVehicle("Onix", "Chevrolet", 2022, "JKL3456")
    ];

    public static IReadOnlyList<Vehicle> VehiclesList => Vehicles;

    private static Vehicle CreateVehicle(string model, string brand, int manufactureYear, string licensePlate)
    {
        var vehicle = (Vehicle) Activator.CreateInstance(typeof(Vehicle), true)!;
        vehicle.Update(manufactureYear, licensePlate, brand, model);
        return vehicle;
    }
}
