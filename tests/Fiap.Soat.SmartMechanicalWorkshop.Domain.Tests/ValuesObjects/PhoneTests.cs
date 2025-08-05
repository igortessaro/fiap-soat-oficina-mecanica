using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.ValuesObjects;

public sealed class PhoneTests
{
    [Fact]
    public void Constructor_ShouldSetAreaCodeAndNumber_FromSingleString()
    {
        // Arrange
        const string input = "11 912345678";

        // Act
        var phone = new Phone(input);

        // Assert
        phone.AreaCode.Should().Be("11");
        phone.Number.Should().Be("912345678");
    }

    [Fact]
    public void Constructor_ShouldSetAreaCodeAndNumber_FromTwoStrings()
    {
        // Arrange
        const string areaCode = "21";
        const string number = "99887766";

        // Act
        var phone = new Phone(areaCode, number);

        // Assert
        phone.AreaCode.Should().Be(areaCode);
        phone.Number.Should().Be(number);
    }

    [Fact]
    public void Constructor_ShouldSetNumberEmpty_WhenNoNumberProvided()
    {
        // Arrange
        const string input = "31";

        // Act
        var phone = new Phone(input);

        // Assert
        phone.AreaCode.Should().Be("31");
        phone.Number.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Constructor_ShouldSetEmpty_WhenInputIsNullOrWhitespace(string? input)
    {
        // Act
        var phone = new Phone(input);

        // Assert
        phone.AreaCode.Should().BeEmpty();
        phone.Number.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitOperator_FromString_ShouldWork()
    {
        // Arrange
        const string input = "41 12345678";

        // Act
        Phone phone = input;

        // Assert
        phone.AreaCode.Should().Be("41");
        phone.Number.Should().Be("12345678");
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldWork()
    {
        // Arrange
        var phone = new Phone("51", "98765432");

        // Act
        string value = phone;

        // Assert
        value.Should().Be("51 98765432");
    }

    [Fact]
    public void Value_ShouldBeEmpty_ByDefault()
    {
        var phone = (Phone) Activator.CreateInstance(typeof(Phone), true)!;
        phone.AreaCode.Should().BeEmpty();
        phone.Number.Should().BeEmpty();
    }
}
