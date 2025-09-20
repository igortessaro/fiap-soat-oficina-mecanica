using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.Vehicles;

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
        var client = PeopleFactory.CreateClient();
        await dbContext.People.AddAsync(client);
        await dbContext.SaveChangesAsync();
        var vehicle = VehicleFactory.CreateVehicle();
        vehicle.GetType().GetProperty("PersonId")!.SetValue(vehicle, client.Id);
        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{vehicle.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
