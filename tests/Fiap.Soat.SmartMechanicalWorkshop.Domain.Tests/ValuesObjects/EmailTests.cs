using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.ValuesObjects;

public sealed class EmailTests
{
    [Fact]
    public void ImplicitOperator_FromString_ShouldSetValue()
    {
        // Arrange
        const string value = "test@example.com";

        // Act
        Email email = value;

        // Assert
        email.Address.Should().Be(value);
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnValue()
    {
        // Arrange
        Email email = "test@example.com";

        // Act
        string value = email;

        // Assert
        value.Should().Be("test@example.com");
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name+tag@sub.domain.com")]
    [InlineData("user_name@domain.co")]
    public void IsValid_ShouldReturnTrue_ForValidEmails(string validEmail)
    {
        Email email = validEmail;
        email.IsValid().Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("plainaddress")]
    [InlineData("missingatsign.com")]
    [InlineData("user@.com")]
    [InlineData("@domain.com")]
    [InlineData("user@domain")]
    public void IsValid_ShouldReturnFalse_ForInvalidEmails(string? invalidEmail)
    {
        Email email = invalidEmail;
        email.IsValid().Should().BeFalse();
    }

    [Fact]
    public void Value_ShouldBeEmpty_ByDefault()
    {
        var email = (Email) Activator.CreateInstance(typeof(Email), true)!;
        email.Address.Should().BeEmpty();
    }
}
