using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.Supplies;

[TestCaseOrderer("Namespace.AlphabeticalOrderer", "AssemblyName")]
public class GetSuppliesTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/supplies";

    [Fact]
    public async Task B010_GetOneAsync_WhenSupplyNotFound_ShouldReturn404()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task C011_GetOneAsync_WhenSupplyFound_ShouldReturn200()
    {
        // Arrange
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var supply = new Supply("GetOneAsync Supply", 100, 10);
        await dbContext.Supplies.AddAsync(supply);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{supply.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
