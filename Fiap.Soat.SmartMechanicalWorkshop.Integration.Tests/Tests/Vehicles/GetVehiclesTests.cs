using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Helpers;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.Vehicles;

[TestCaseOrderer("Namespace.AlphabeticalOrderer", "AssemblyName")]
public sealed class GetVehiclesTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/vehicles";

    [Fact]
    public async Task UC010_GetOneAsync_WhenVehicleNotFound_ShouldReturn404()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task US011_GetOneAsync_WhenVehicleFound_ShouldReturn200()
    {
        // Arrange
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var client = new Person(
            "Client A",
            "999.999.999-99",
            EPersonType.Client,
            null,
            "client@email.com",
            new Phone("55", "99999999"),
            new Address("Street", "City", "State", "99999-999"));
        await dbContext.People.AddAsync(client);
        await dbContext.SaveChangesAsync();
        var vehicle = VehicleHelper.VehiclesList[0];
        vehicle.GetType().GetProperty("PersonId")!.SetValue(vehicle, client.Id);
        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{vehicle.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
