using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.AvailableServices;

public sealed class GetAvailableServicesTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/availableservices";

    [Fact]
    public async Task B010_GetOneAsync_WhenAvailableServiceNotFound_ShouldReturn404()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task C011_GetOneAsync_WhenAvailableServiceFound_ShouldReturn200()
    {
        // Arrange
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var availableService = AvailableServiceFactory.AvailableServices[0];
        await dbContext.AvailableServices.AddAsync(availableService);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{availableService.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
