using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed partial class ServiceOrderServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<ILogger<ServiceOrderService>> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IServiceOrderEventRepository> _repositoryEventsMock = new();
    private readonly Mock<IServiceOrderRepository> _repositoryMock = new();
    private readonly Mock<IPersonRepository> _personRepositoryMock = new();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new();
    private readonly Mock<IAvailableServiceRepository> _availableServiceRepositoryMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<IEmailTemplateProvider> _emailTemplateProviderMock = new();
    private readonly ServiceOrderService _service;

    public ServiceOrderServiceTests()
    {
        _service = new ServiceOrderService(
            _loggerMock.Object,
            _mapperMock.Object,
            _repositoryMock.Object,
            _repositoryEventsMock.Object,
            _personRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _availableServiceRepositoryMock.Object,
            _emailServiceMock.Object,
            _emailTemplateProviderMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        var request = _fixture.Create<CreateServiceOrderRequest>();
        var entity = _fixture.Create<ServiceOrder>();
        _mapperMock.Setup(m => m.Map<ServiceOrder>(request)).Returns(entity);
        _personRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _service.CreateAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenVehicleDoesNotExist()
    {
        var request = _fixture.Create<CreateServiceOrderRequest>();
        var entity = _fixture.Create<ServiceOrder>();
        _mapperMock.Setup(m => m.Map<ServiceOrder>(request)).Returns(entity);
        _personRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _vehicleRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _service.CreateAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNotFound_WhenAnyServiceNotFound()
    {
        var request = _fixture.Build<CreateServiceOrderRequest>()
            .With(x => x.ServiceIds, [Guid.NewGuid()])
            .Create();
        var entity = _fixture.Create<ServiceOrder>();
        _mapperMock.Setup(m => m.Map<ServiceOrder>(request)).Returns(entity);
        _personRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _vehicleRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _availableServiceRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        var result = await _service.CreateAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenValid()
    {
        var request = _fixture.Build<CreateServiceOrderRequest>()
            .With(x => x.ServiceIds, [Guid.NewGuid()])
            .Create();
        var entity = _fixture.Create<ServiceOrder>();
        var availableService = _fixture.Create<AvailableService>();
        var createdEntity = _fixture.Create<ServiceOrder>();
        var dto = _fixture.Create<ServiceOrderDto>();

        _mapperMock.Setup(m => m.Map<ServiceOrder>(request)).Returns(entity);
        _personRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _vehicleRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _availableServiceRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(availableService);
        _repositoryMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).ReturnsAsync(createdEntity);
        _mapperMock.Setup(m => m.Map<ServiceOrderDto>(createdEntity)).Returns(dto);

        var result = await _service.CreateAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenFound()
    {
        var id = Guid.NewGuid();
        var entity = _fixture.Create<ServiceOrder>();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);

        var result = await _service.DeleteAsync(id, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceOrder?) null);

        var result = await _service.DeleteAsync(id, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnDto_WhenFound()
    {
        var id = Guid.NewGuid();
        var entity = _fixture.Create<ServiceOrder>();
        var dto = _fixture.Create<ServiceOrderDto>();
        _repositoryMock.Setup(r => r.GetDetailedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<ServiceOrderDto>(entity)).Returns(dto);

        var result = await _service.GetOneAsync(id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task GetOneAsync_ShouldReturnNotFound_WhenNotFound()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetDetailedAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceOrder?) null);

        var result = await _service.GetOneAsync(id, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenNotFound()
    {
        var input = _fixture.Create<UpdateOneServiceOrderInput>();
        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceOrder?) null);

        var result = await _service.UpdateAsync(input, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenAvailableServiceNotFound()
    {
        var input = _fixture.Build<UpdateOneServiceOrderInput>()
            .With(x => x.ServiceIds, [Guid.NewGuid()])
            .Create();
        var entity = _fixture.Create<ServiceOrder>();
        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _availableServiceRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((AvailableService?) null);

        var result = await _service.UpdateAsync(input, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdated_WhenValid()
    {
        var serviceId = Guid.NewGuid();
        var input = _fixture.Build<UpdateOneServiceOrderInput>()
            .With(x => x.ServiceIds, [serviceId])
            .Create();
        var entity = _fixture.Create<ServiceOrder>();
        var availableService = _fixture.Create<AvailableService>();
        var updatedEntity = _fixture.Create<ServiceOrder>();
        var dto = _fixture.Create<ServiceOrderDto>();

        _repositoryMock.Setup(r => r.GetAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _availableServiceRepositoryMock.Setup(r => r.GetByIdAsync(serviceId, It.IsAny<CancellationToken>())).ReturnsAsync(availableService);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>())).ReturnsAsync(updatedEntity);
        _mapperMock.Setup(m => m.Map<ServiceOrderDto>(updatedEntity)).Returns(dto);

        var result = await _service.UpdateAsync(input, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dto);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginated()
    {
        var paginatedRequest = _fixture.Create<PaginatedRequest>();
        var paginate = _fixture.Create<Paginate<ServiceOrder>>();
        var paginateDto = _fixture.Create<Paginate<ServiceOrderDto>>();

        _repositoryMock.Setup(r => r.GetAllAsync(paginatedRequest, It.IsAny<CancellationToken>())).ReturnsAsync(paginate);
        _mapperMock.Setup(m => m.Map<Paginate<ServiceOrderDto>>(paginate)).Returns(paginateDto);

        var result = await _service.GetAllAsync(null, paginatedRequest, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(paginateDto);
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldReturnNotFound_WhenNotFound()
    {
        var request = _fixture.Create<SendServiceOrderApprovalRequest>();
        _repositoryMock.Setup(r => r.GetDetailedAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceOrder?) null);

        var result = await _service.SendForApprovalAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldReturnAccepted_WhenEmailSent()
    {
        var request = _fixture.Create<SendServiceOrderApprovalRequest>();
        var entity = _fixture.Create<ServiceOrder>();
        entity.GetType().GetProperty("Client")!.SetValue(entity, _fixture.Create<Person>());
        _repositoryMock.Setup(r => r.GetDetailedAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _emailTemplateProviderMock.Setup(e => e.GetTemplate(entity)).Returns("html");
        _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        var result = await _service.SendForApprovalAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.Accepted);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldReturnInternalServerError_WhenEmailFails()
    {
        var request = _fixture.Create<SendServiceOrderApprovalRequest>();
        var entity = _fixture.Create<ServiceOrder>();
        entity.GetType().GetProperty("Client")!.SetValue(entity, _fixture.Create<Person>());
        _repositoryMock.Setup(r => r.GetDetailedAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        _emailTemplateProviderMock.Setup(e => e.GetTemplate(entity)).Returns("html");
        _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        var result = await _service.SendForApprovalAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task PatchAsync_ShouldReturnNotFound_WhenNotFound()
    {
        var input = _fixture.Create<UpdateOneServiceOrderInput>();
        _repositoryMock.Setup(r => r.GetByIdAsync(input.Id, It.IsAny<CancellationToken>())).ReturnsAsync((ServiceOrder?) null);

        var result = await _service.PatchAsync(input, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetExecutionTimeAsync_ShouldReturnZero_WhenNotFound()
    {
        var input = _fixture.Create<UpdateOneServiceOrderInput>();
        _repositoryEventsMock.Setup(r => r.GetAverageExecutionTime(It.IsAny<CancellationToken>())).ReturnsAsync(TimeSpan.Zero);

        var result = await _service.GetAverageExecutionTime( CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().Be(TimeSpan.Zero);
    }
}
