using AutoFixture;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Person;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Tests.Controllers;

public sealed class PeopleControllerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly PeopleController _controller;

    public PeopleControllerTests()
    {
        _controller = new PeopleController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnOkResult_WhenPersonExists()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = new Response<PersonDto>(_fixture.Create<PersonDto>(), HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetOneAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkResult_WithPaginatedPeople()
    {
        // Arrange
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var people = _fixture.CreateMany<PersonDto>(5).ToList();
        var paginatedResponse = new Paginate<PersonDto>(people, people.Count, paginatedRequest.PageSize, paginatedRequest.PageNumber, 1);
        var response = new Response<Paginate<PersonDto>>(paginatedResponse, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListPeopleQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAllAsync(paginatedRequest, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResult_WhenPersonIsCreated()
    {
        // Arrange
        var request = _fixture.Create<CreatePersonRequest>();
        var response = new Response<PersonDto>(_fixture.Create<PersonDto>(), HttpStatusCode.Created);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateAsync(request, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.Created);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenPersonIsDeleted()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var response = ResponseFactory.Ok(HttpStatusCode.NoContent);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.DeleteAsync(id, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        objectResult.Value.Should().Be(response);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenPersonIsUpdated()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateOnePersonRequest>();
        var dto = _fixture.Create<PersonDto>();
        var response = new Response<PersonDto>(dto, HttpStatusCode.OK);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.UpdateAsync(id, request, CancellationToken.None);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be((int) HttpStatusCode.OK);
        objectResult.Value.Should().Be(response);
    }
}
