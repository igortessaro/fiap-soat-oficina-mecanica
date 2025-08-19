using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Models.Authentication;

[ExcludeFromCodeCoverage]
public record LoginRequest(string Email, string Password);
