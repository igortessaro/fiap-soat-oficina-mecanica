using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Authentication;

[ExcludeFromCodeCoverage]
public record LoginRequest(string Email, string Password);
