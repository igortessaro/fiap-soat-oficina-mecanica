using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Authentication.Login;

public sealed class LoginHandler(IConfiguration config, IPersonRepository personRepository) : IRequestHandler<LoginCommand, Response<string>>
{
    public async Task<Response<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var person = await personRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (person is null)
        {
            return ResponseFactory.Fail<string>($"Person with {request.Email} not found", HttpStatusCode.NotFound);
        }

        if (!person.Password.VerifyPassword(request.Password))
        {
            return ResponseFactory.Fail<string>("Invalid login credentials", HttpStatusCode.Unauthorized);
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
