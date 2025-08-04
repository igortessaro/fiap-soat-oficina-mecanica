using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Entities;

public sealed class AvailableServiceTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        const string name = "Oil Change";
        const decimal price = 99.99m;

        // Act
        var service = new AvailableService(name, price);

        // Assert
        service.Name.Should().Be(name);
        service.Price.Should().Be(price);
        service.ServiceOrders.Should().BeEmpty();
        service.AvailableServiceSupplies.Should().BeEmpty();
    }

    [Fact]
    public void Update_ShouldUpdateNameAndPrice_WhenValid()
    {
        // Arrange
        var service = new AvailableService("Old", 10m);

        // Act
        service.Update("New", 20m);

        // Assert
        service.Name.Should().Be("New");
        service.Price.Should().Be(20m);
    }

    [Fact]
    public void Update_ShouldNotUpdateName_WhenNameIsNullOrEmpty()
    {
        // Arrange
        var service = new AvailableService("Name", 10m);

        // Act
        service.Update("", 20m);

        // Assert
        service.Name.Should().Be("Name");
        service.Price.Should().Be(20m);

        // Act
        service.Update(null, 30m);

        // Assert
        service.Name.Should().Be("Name");
        service.Price.Should().Be(30m);
    }

    [Fact]
    public void Update_ShouldNotUpdatePrice_WhenPriceIsNull()
    {
        // Arrange
        var service = new AvailableService("Name", 10m);

        // Act
        service.Update("NewName", null);

        // Assert
        service.Name.Should().Be("NewName");
        service.Price.Should().Be(10m);
    }

    [Fact]
    public void AddSupply_ShouldAddSupplyToCollection()
    {
        // Arrange
        var service = new AvailableService("Name", 10m);
        var supplyId = Guid.NewGuid();
        const int quantity = 5;

        // Act
        service.AddSupply(supplyId, quantity);

        // Assert
        service.AvailableServiceSupplies.Should().ContainSingle();
        var supply = service.AvailableServiceSupplies.First();
        supply.AvailableServiceId.Should().Be(service.Id);
        supply.SupplyId.Should().Be(supplyId);
        supply.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void AddSupplies_ShouldClearAndAddSupplies()
    {
        // Arrange
        var service = new AvailableService("Name", 10m);
        service.AddSupply(Guid.NewGuid(), 1);

        var supplies = new List<ServiceSupplyDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), 2),
            new(Guid.NewGuid(), Guid.NewGuid(), 3)
        };

        // Act
        service.AddSupplies(supplies);

        // Assert
        service.AvailableServiceSupplies.Should().HaveCount(2);
        service.AvailableServiceSupplies.Select(s => s.Quantity).Should().BeEquivalentTo([2, 3]);
    }

    [Fact]
    public void AddSupplies_ShouldClearSupplies_WhenEmptyList()
    {
        // Arrange
        var service = new AvailableService("Name", 10m);
        service.AddSupply(Guid.NewGuid(), 1);

        // Act
        service.AddSupplies(new List<ServiceSupplyDto>());

        // Assert
        service.AvailableServiceSupplies.Should().BeEmpty();
    }
}
