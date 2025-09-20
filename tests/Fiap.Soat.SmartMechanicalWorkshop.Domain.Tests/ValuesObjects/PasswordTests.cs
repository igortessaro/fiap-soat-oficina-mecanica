using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using FluentAssertions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Tests.ValuesObjects;

public sealed class PasswordTests
{
    [Theory]
    [InlineData("Abcdef1!")]
    [InlineData("Password123!")]
    [InlineData("!Qwerty9")]
    public void IsValid_ShouldReturnTrue_ForValidPasswords(string validPassword)
    {
        Password.IsValid(validPassword).Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("short1!")]
    [InlineData("abcdefgh")]
    [InlineData("12345678")]
    [InlineData("abcd1234")]
    [InlineData("abcd!@#$")]
    public void IsValid_ShouldReturnFalse_ForInvalidPasswords(string invalidPassword)
    {
        Password.IsValid(invalidPassword).Should().BeFalse();
    }

    [Fact]
    public void ImplicitOperator_FromString_ShouldHashPassword()
    {
        Password password = "Abcdef1!";
        password.Value.Should().NotBeNullOrEmpty();
        password.Value.Should().Contain(".");
    }

    [Fact]
    public void ImplicitOperator_ToString_ShouldReturnHashedValue()
    {
        Password password = "Abcdef1!";
        string value = password;
        value.Should().Be(password.Value);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        Password password = "Abcdef1!";
        password.VerifyPassword("Abcdef1!").Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        Password password = "Abcdef1!";
        password.VerifyPassword("WrongPass1!").Should().BeFalse();
    }

    [Fact]
    public void Value_ShouldBeEmpty_ByDefault()
    {
        var password = (Password) Activator.CreateInstance(typeof(Password), true)!;
        password.Value.Should().BeEmpty();
    }
}
