using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class AuthService(IConfiguration config, IPersonRepository personRepository) : IAuthService
{
    public async Task<Response<string>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var person = await personRepository.GetByEmailAsync(loginRequest.Email, cancellationToken);
        if (person is null)
        {
            return ResponseFactory.Fail<string>(new FluentResults.Error($"Person with {loginRequest.Email} not found"), HttpStatusCode.NotFound);
        }

        if (!person.Password.VerifyPassword(loginRequest.Password))
        {
            return ResponseFactory.Fail<string>(new FluentResults.Error("Invalid login credentials"), HttpStatusCode.Unauthorized);
        }

        string token = GenerateJwtToken(person);
        return ResponseFactory.Ok(token);
    }

    private string GenerateJwtToken(Person person)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: null,
            claims:
            [
                new Claim(ClaimTypes.Name, person.Fullname),
                new Claim(ClaimTypes.Email, person.Email),
                new Claim("PersonType", person.PersonType.ToString()),
                new Claim(ClaimTypes.Role, person.EmployeeRole?.ToString() ?? string.Empty),
            ],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
