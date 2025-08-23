using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;

public interface IAuthenticationController
{
    Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken);
}
