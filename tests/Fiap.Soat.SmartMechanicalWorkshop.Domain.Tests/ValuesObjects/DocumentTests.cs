using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.ValuesObjects;

public sealed class DocumentTests
{
    [Fact]
    public void ImplicitOperator_FromString_ShouldSetValue()
    {
        // Arrange
        const string value = "12345678901";

        // Act
        Document doc = value;

        // Assert
        doc.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnValue()
    {
        // Arrange
        Document doc = "12345678901";

        // Act
        string value = doc;

        // Assert
        value.Should().Be("12345678901");
    }

    [Theory]
    [InlineData("111.444.777-35")] // valid CPF
    [InlineData("11144477735")]    // valid CPF, no mask
    public void IsValid_ShouldReturnTrue_ForValidCpf(string cpf)
    {
        Document doc = cpf;
        doc.IsValid().Should().BeTrue();
    }

    [Theory]
    [InlineData("12.345.678/0001-95")] // valid CNPJ
    [InlineData("12345678000195")]     // valid CNPJ, no mask
    public void IsValid_ShouldReturnTrue_ForValidCnpj(string cnpj)
    {
        Document doc = cnpj;
        doc.IsValid().Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("00000000000")] // invalid CPF (all same digits)
    [InlineData("1234567890")]  // invalid CPF (too short)
    [InlineData("123456789012")] // invalid CNPJ (too short)
    [InlineData("00000000000000")] // invalid CNPJ (all same digits)
    [InlineData("notanumber")]
    public void IsValid_ShouldReturnFalse_ForInvalidDocuments(string? value)
    {
        Document doc = value;
        doc.IsValid().Should().BeFalse();
    }

    [Fact]
    public void Value_ShouldBeEmpty_ByDefault()
    {
        var doc = (Document) Activator.CreateInstance(typeof(Document), true)!;
        doc.Value.Should().BeEmpty();
    }
}
