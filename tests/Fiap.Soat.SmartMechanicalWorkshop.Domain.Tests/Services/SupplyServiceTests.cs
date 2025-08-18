using AutoFixture;
using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentAssertions;
using Moq;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Services;

public sealed class SupplyServiceTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISupplyRepository> _repositoryMock = new();
    private readonly SupplyService _service;

    public SupplyServiceTests()
    {
        _service = new SupplyService(_repositoryMock.Object, _mapperMock.Object);
    }


}
