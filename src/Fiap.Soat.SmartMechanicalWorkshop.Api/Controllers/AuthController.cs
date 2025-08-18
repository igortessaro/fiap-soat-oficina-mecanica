using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Authentication.Login;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="login">Login credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    ///     200: Returns JWT token on successful authentication.<br />
    ///     401: Returns error details if authentication fails.
    /// </returns>
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Authenticate user",
        Description = "Authenticates a user and returns a JWT token if successful."
    )]
    [SwaggerResponse(200, "Returns JWT token on successful authentication.", typeof(Response<string>))]
    [SwaggerResponse(404, "Returns when user/password is not found", typeof(NotFoundResult))]
    public async Task<IActionResult> Login([FromBody] LoginRequest login, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginCommand(login.Email, login.Password), cancellationToken);
        return response.ToActionResult();
    }
}
