using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.Supplies;

public sealed class CreateSuppliesTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/supplies";

    [Fact]
    public async Task A0001_CreateAsync_WhenCreateAvailableService_ShouldReturn201()
    {
        // Arrange
        var toCreate = new CreateSupplyCommand("Integration Tests", 99, 0);
        string json = JsonConvert.SerializeObject(toCreate);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
