using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
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
        var toCreate = new CreateNewSupplyRequest { Name = "Integration Tests", Price = 99, Quantity = 0 };
        string json = JsonConvert.SerializeObject(toCreate);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
