using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Tests.People;

[TestCaseOrderer("Namespace.AlphabeticalOrderer", "AssemblyName")]
public sealed class GetOnePersonTest : CustomWebApplicationFactory<Program>
{
    private const string Endpoint = "/api/v1/people";

    [Fact]
    public async Task UC010_GetOneAsync_WhenPersonNotFound_ShouldReturn404()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var client = CreateClient();

        // Act
        var response = await client.GetAsync($"{Endpoint}/{personId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task US011_GetOneAsync_WhenPersonFound_ShouldReturn200()
    {
        // Arrange
        var client = CreateClient();
        var peopleHttpMessage = await client.GetAsync(Endpoint);
        var people = Newtonsoft.Json.JsonConvert.DeserializeObject<Response<Paginate<PersonDto>>>(await peopleHttpMessage.Content.ReadAsStringAsync());
        var personId = people?.Data.Items.First().Id;

        // Act
        var response = await client.GetAsync($"{Endpoint}/{personId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
