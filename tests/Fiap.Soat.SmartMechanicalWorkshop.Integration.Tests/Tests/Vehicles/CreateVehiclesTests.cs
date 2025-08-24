using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.Vehicles;

public sealed class CreateVehiclesTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/vehicles";

    [Fact]
    public async Task Vehicles_CreateAsync_WhenPersonNotFound_ShouldReturn404()
    {
        // Arrange
        const string expectedMessage = "Person not found";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        var request = new CreateNewVehicleRequest
        {
            PersonId = Guid.NewGuid(),
            LicensePlate = VehicleFactory.CreateValidLicensePlate(),
            Model = Faker.Vehicle.Model(),
            ManufactureYear = DateTime.Now.Year,
            Brand = Faker.Vehicle.Manufacturer()
        };
        string json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);
        string responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains(expectedMessage, responseContent);
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Fact]
    public async Task Vehicles_CreateAsync_WhenPersonIsNotClient_ShouldReturn400()
    {
        // Arrange
        const string expectedMessage = "Only clients are allowed to register a vehicle";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var client = PeopleFactory.CreateDetailerEmployee();
        await dbContext.People.AddRangeAsync(client);
        await dbContext.SaveChangesAsync();
        var request = new CreateNewVehicleRequest
        {
            PersonId = client.Id,
            LicensePlate = VehicleFactory.CreateValidLicensePlate(),
            Model = Faker.Vehicle.Model(),
            ManufactureYear = DateTime.Now.Year,
            Brand = Faker.Vehicle.Manufacturer()
        };
        string json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);
        string responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains(expectedMessage, responseContent);
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Fact]
    public async Task Vehicles_CreateAsync_WhenLicensePlatIsInvalid_ShouldReturn400()
    {
        // Arrange
        const string expectedMessage = "Invalid license plate format";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var client = PeopleFactory.CreateClient();
        await dbContext.People.AddRangeAsync(client);
        await dbContext.SaveChangesAsync();
        var request = new CreateNewVehicleRequest
        {
            PersonId = client.Id,
            LicensePlate = VehicleFactory.CreateInvalidLicensePlate(),
            Model = Faker.Vehicle.Model(),
            ManufactureYear = DateTime.Now.Year,
            Brand = Faker.Vehicle.Manufacturer()
        };
        string json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);
        string responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains(expectedMessage, responseContent);
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Fact]
    public async Task Vehicles_CreateAsync_WhenCreatedVehicle_ShouldReturn201()
    {
        // Arrange
        const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var client = PeopleFactory.CreateClient();
        await dbContext.People.AddRangeAsync(client);
        await dbContext.SaveChangesAsync();
        var request = new CreateNewVehicleRequest
        {
            PersonId = client.Id,
            LicensePlate = VehicleFactory.CreateValidLicensePlate(),
            Model = Faker.Vehicle.Model(),
            ManufactureYear = DateTime.Now.Year,
            Brand = Faker.Vehicle.Manufacturer()
        };
        string json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);
        string responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }
}
