using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.Entities;

public sealed class AddressTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        const string street = "Main St";
        const string city = "Metropolis";
        const string state = "NY";
        const string zipCode = "12345";

        // Act
        var address = new Address(street, city, state, zipCode);

        // Assert
        address.Street.Should().Be(street);
        address.City.Should().Be(city);
        address.State.Should().Be(state);
        address.ZipCode.Should().Be(zipCode);
    }

    [Fact]
    public void Update_ShouldNotChange_WhenArgumentIsNull()
    {
        // Arrange
        var address = new Address("A", "B", "C", "D");

        // Act
        address.Update(null);

        // Assert
        address.Street.Should().Be("A");
        address.City.Should().Be("B");
        address.State.Should().Be("C");
        address.ZipCode.Should().Be("D");
    }

    [Fact]
    public void Update_ShouldUpdateAllFields_WhenAllAreNonEmpty()
    {
        // Arrange
        var address = new Address("A", "B", "C", "D");
        var newAddress = new Address("X", "Y", "Z", "W");

        // Act
        address.Update(newAddress);

        // Assert
        address.Street.Should().Be("X");
        address.City.Should().Be("Y");
        address.State.Should().Be("Z");
        address.ZipCode.Should().Be("W");
    }

    [Fact]
    public void Update_ShouldNotUpdateFields_WhenNewValuesAreEmpty()
    {
        // Arrange
        var address = new Address("A", "B", "C", "D");
        var newAddress = new Address("", "", "", "");

        // Act
        address.Update(newAddress);

        // Assert
        address.Street.Should().Be("A");
        address.City.Should().Be("B");
        address.State.Should().Be("C");
        address.ZipCode.Should().Be("D");
    }

    [Fact]
    public void Update_ShouldUpdateOnlyNonEmptyFields()
    {
        // Arrange
        var address = new Address("A", "B", "C", "D");
        var newAddress = new Address("X", "", "Z", "");

        // Act
        address.Update(newAddress);

        // Assert
        address.Street.Should().Be("X");
        address.City.Should().Be("B");
        address.State.Should().Be("Z");
        address.ZipCode.Should().Be("D");
    }
}
