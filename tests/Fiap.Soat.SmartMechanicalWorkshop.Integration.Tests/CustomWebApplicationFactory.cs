using Bogus;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected readonly HttpClient Client;
    protected readonly Faker Faker = new Faker("pt_BR");
    private readonly string JwtSigningKey = "integration-tests-jwt-signing-key";

    protected CustomWebApplicationFactory()
    {
        Client = CreateClient();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "smartworkshop.com",
            audience: null,
            claims:
            [
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Email, "testuser@email.com"),
                new Claim("PersonType", nameof(PersonType.Client)),
                new Claim(ClaimTypes.Role, nameof(EmployeeRole.VehicleInspector))
            ],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var jwtSecurity = new JwtSecurityTokenHandler().WriteToken(token);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtSecurity);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("Jwt__Key", JwtSigningKey);
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });

        builder.UseEnvironment("IntegrationTest");
    }
}
