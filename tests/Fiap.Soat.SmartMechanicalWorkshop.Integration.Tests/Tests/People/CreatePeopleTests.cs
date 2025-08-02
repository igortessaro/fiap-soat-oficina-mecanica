using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Net;
using System.Net.Http.Headers;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.People;

public sealed class CreatePeopleTests : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/people";

    [Fact]
    public async Task UC0001_CreateAsync_WhenCreatePerson_ShouldReturn201()
    {
        // Arrange
        var personToCreate = new CreatePersonRequest(
            "Integration Test Person",
            "12345678901",
            EPersonType.Client,
            null,
            "integration.test@person.com",
            "Password123!",
            new CreatePhoneRequest("11", "912345678"),
            new CreateAddressRequest("Rua Exemplo", "São Paulo", "SP", "01234-567"));
        string personJson = Newtonsoft.Json.JsonConvert.SerializeObject(personToCreate);
        var content = new StringContent(personJson, MediaTypeHeaderValue.Parse("application/json"));

        // Act
        var response = await Client.PostAsync(Endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
