using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IAuthService
{
    Task<Response<string>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
}
