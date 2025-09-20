using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;

public interface IAuthenticationController
{
    Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken);
}
