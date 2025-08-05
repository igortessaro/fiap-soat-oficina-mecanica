using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.ValuesObjects;

public sealed class LicensePlateTests
{
    [Fact]
    public void ImplicitOperator_FromString_ShouldSetValue()
    {
        // Arrange
        const string value = "ABC1D23";

        // Act
        LicensePlate plate = value;

        // Assert
        plate.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnValue()
    {
        // Arrange
        LicensePlate plate = "ABC1D23";

        // Act
        string value = plate;

        // Assert
        value.Should().Be("ABC1D23");
    }

    [Theory]
    [InlineData("ABC1234")]   // Old format
    [InlineData("ABC1D23")]   // Mercosul format
    [InlineData("abc1234")]   // Lowercase, should be valid if logic is case-insensitive
    [InlineData("abc1d23")]   // Lowercase Mercosul
    public void IsValid_ShouldReturnTrue_ForValidPlates(string validPlate)
    {
        LicensePlate plate = validPlate;
        plate.IsValid().Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("A1B2C3D")]
    [InlineData("1234567")]
    [InlineData("ABCD123")]
    [InlineData("AB12345")]
    [InlineData("A1B2C3")]
    [InlineData("!@#1234")]
    public void IsValid_ShouldReturnFalse_ForInvalidPlates(string? invalidPlate)
    {
        LicensePlate plate = invalidPlate;
        plate.IsValid().Should().BeFalse();
    }

    [Fact]
    public void Value_ShouldBeEmpty_ByDefault()
    {
        var plate = (LicensePlate) Activator.CreateInstance(typeof(LicensePlate), true)!;
        plate.Value.Should().BeEmpty();
    }
}
