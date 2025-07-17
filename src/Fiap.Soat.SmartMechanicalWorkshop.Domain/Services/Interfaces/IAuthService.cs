using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Threading;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
public interface IAuthService
{
    Task<Response<string>> Login(LoginRequest loginRequest, CancellationToken cancellationToken);
}
