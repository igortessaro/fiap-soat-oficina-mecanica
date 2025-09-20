using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.People;

public sealed class GetPeopleTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/people";

    [Fact]
    public async Task UC010_GetOneAsync_WhenPersonNotFound_ShouldReturn404()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task US011_GetOneAsync_WhenPersonFound_ShouldReturn200()
    {
        // Arrange
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var client = PeopleFactory.CreateClient();
        await dbContext.People.AddRangeAsync(client);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{client.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
