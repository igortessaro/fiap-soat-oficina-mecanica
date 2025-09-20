using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Authentication;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Authentication.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;

public sealed class AuthenticationController(IMediator mediator) : IAuthenticationController
{
    public async Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginCommand(login.Email, login.Password), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }
}
