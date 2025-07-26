using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.ServiceOrders;

public sealed class GetServiceOrdersTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/serviceorders";

    [Fact]
    public async Task UC010_GetOneAsync_WhenServiceOrderNotFound_ShouldReturn404()
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
            "Client Service Order",
            "999.999.999-99",
            EPersonType.Client,
            null,
            "client@email.com",
            new Phone("55", "99999999"),
            new Address("Street", "City", "State", "99999-999"));
        await dbContext.People.AddAsync(client);
        await dbContext.SaveChangesAsync();
        var vehicle = new Vehicle("Model 001", "Brand 001", DateTime.UtcNow.Year, "ABC1234", client.Id);
        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();
        var serviceOrder = ServiceOrderFactory.ServiceOrders[0];
        serviceOrder.GetType().GetProperty("ClientId")!.SetValue(serviceOrder, client.Id);
        serviceOrder.GetType().GetProperty("VehicleId")!.SetValue(serviceOrder, vehicle.Id);
        await dbContext.ServiceOrders.AddAsync(serviceOrder);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{serviceOrder.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
