using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;

[ExcludeFromCodeCoverage]
public record LoginRequest(string Email, string Password);
