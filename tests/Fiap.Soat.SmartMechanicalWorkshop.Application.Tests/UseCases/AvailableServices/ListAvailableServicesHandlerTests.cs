using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Tests.UseCases.AvailableServices
{
    public sealed class ListAvailableServicesHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IAvailableServiceRepository> _repositoryMock = new();
        private readonly ListAvailableServicesHandler _useCase;

        public ListAvailableServicesHandlerTests()
        {
            _useCase = new ListAvailableServicesHandler(_mapperMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginated()
        {
            // Arrange
            var paginatedRequest = _fixture.Create<ListAvailableServicesQuery>();
            var paginate = _fixture.Create<Paginate<AvailableService>>();
            var paginateDto = _fixture.Create<Paginate<AvailableServiceDto>>();

            _repositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<IReadOnlyList<string>>(),
                    It.IsAny<PaginatedRequest>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<AvailableService>, IOrderedQueryable<AvailableService>>>()))
                .ReturnsAsync(paginate);

            // Necessário porque o handler acessa mapper.ConfigurationProvider.ToString()
            _mapperMock
                .SetupGet(m => m.ConfigurationProvider)
                .Returns(Mock.Of<IConfigurationProvider>());

            // Act
            var result = await _useCase.Handle(paginatedRequest, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(paginate);
        }
    }
}
