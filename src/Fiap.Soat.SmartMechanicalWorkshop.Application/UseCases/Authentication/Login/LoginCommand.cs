using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Authentication.Login;

public record LoginCommand(string Email, string Password) : IRequest<Response<string>>;
