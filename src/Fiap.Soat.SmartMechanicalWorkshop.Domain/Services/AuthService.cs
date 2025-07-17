using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
public class AuthService(IConfiguration config, IPersonService personService) : IAuthService
{
    private string GenerateJwtToken(PersonDto person)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: null,
            claims: new[]
            {
                new System.Security.Claims.Claim(ClaimTypes.Name, person.Fullname),
                 new System.Security.Claims.Claim(ClaimTypes.Email, person.Email.Address),
                  new System.Security.Claims.Claim("PersonType", person.PersonType.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Role, person.EmployeeRole.ToString())
            },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Response<string>> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var response = await personService.GetOneByLoginAsync(loginRequest, cancellationToken);

        if (response == null || !response.IsSuccess)
        {
            return ResponseFactory.Fail<string>(new FluentResults.Error("Invalid login credentials"), HttpStatusCode.Unauthorized);
        }

        string token = GenerateJwtToken(response.Data);
        return ResponseFactory.Ok(token);
    }
}
