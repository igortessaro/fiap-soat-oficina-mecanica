using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.AvailableServices;

public sealed class CreateAvailableServicesTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/availableservices";

    [Fact]
    public async Task A0002_CreateAsync_WhenCreateAvailableService_ShouldReturn201()
    {
        // Arrange
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var supplies = SupplyFactory.Supplies.Take(3).ToList();
        await dbContext.Supplies.AddRangeAsync(supplies);
        await dbContext.SaveChangesAsync();
        var suppliesRequest = supplies.Select(x => new ServiceSupplyRequest(x.Id, 10)).ToList();
        var toCreate = new CreateAvailableServiceRequest("Available Service Integration Tests", (decimal) 102.09, suppliesRequest);
        string json = JsonConvert.SerializeObject(toCreate);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
