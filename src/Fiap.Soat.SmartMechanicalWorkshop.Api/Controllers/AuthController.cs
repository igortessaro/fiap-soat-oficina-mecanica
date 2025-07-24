using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="login">Login credentials.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// 200: Returns JWT token on successful authentication.<br/>
        /// 401: Returns error details if authentication fails.
        /// </returns>
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Authenticate user",
            Description = "Authenticates a user and returns a JWT token if successful."
        )]
        [SwaggerResponse(200, "Returns JWT token on successful authentication.", typeof(object))]
        [SwaggerResponse(404, "Returns error details if authentication fails.", typeof(Response<string>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest login, CancellationToken cancellationToken)
        {
            var response = await authService.Login(login, cancellationToken);

            return response.ToActionResult();
        }
    }
}
