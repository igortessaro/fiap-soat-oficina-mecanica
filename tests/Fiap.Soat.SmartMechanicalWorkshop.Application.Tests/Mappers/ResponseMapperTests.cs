using Fiap.Soat.SmartMechanicalWorkshop.Application.Mappers;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.Mappers;

public sealed class ResponseMapperTests
{
    [Fact]
    public void Map_ShouldReturnMappedResponse_WhenSuccess()
    {
        // Arrange
        var response = ResponseFactory.Ok(10);

        // Act
        var mapped = ResponseMapper.Map(response, x => x.ToString());

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Data.Should().Be("10");
        mapped.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public void Map_ShouldPropagateError_WhenFailure()
    {
        // Arrange
        var response = ResponseFactory.Fail<int>("error");

        // Act
        var mapped = ResponseMapper.Map(response, x => x.ToString());

        // Assert
        mapped.IsSuccess.Should().BeFalse();
        mapped.Reasons.Should().ContainSingle(r => r.Message == "error");
        mapped.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        mapped.Data.Should().Be(null);
    }

    [Fact]
    public void MapPaginate_ShouldReturnMappedPaginate_WhenSuccess()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };
        var paginate = new Paginate<int>(items, 3, 10, 1, 1);
        var response = ResponseFactory.Ok(paginate);

        // Act
        var mapped = ResponseMapper.Map(response, x => x * 2);

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Data.Items.Should().BeEquivalentTo(new List<int> { 2, 4, 6 });
        mapped.Data.TotalCount.Should().Be(3);
        mapped.Data.PageSize.Should().Be(10);
        mapped.Data.CurrentPage.Should().Be(1);
        mapped.Data.TotalPages.Should().Be(1);
        mapped.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public void MapPaginate_ShouldPropagateError_WhenFailure()
    {
        // Arrange
        var response = ResponseFactory.Fail<Paginate<int>>("fail", HttpStatusCode.NotFound);

        // Act
        var mapped = ResponseMapper.Map(response, x => x * 2);

        // Assert
        mapped.IsSuccess.Should().BeFalse();
        mapped.Reasons.Should().ContainSingle(r => r.Message == "fail");
        mapped.StatusCode.Should().Be(HttpStatusCode.NotFound);
        mapped.Data.Should().BeNull();
    }
}
