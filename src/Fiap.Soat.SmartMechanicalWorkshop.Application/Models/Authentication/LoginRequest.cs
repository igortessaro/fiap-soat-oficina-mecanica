using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Authentication;

[ExcludeFromCodeCoverage]
public record LoginRequest(string Email, string Password);
