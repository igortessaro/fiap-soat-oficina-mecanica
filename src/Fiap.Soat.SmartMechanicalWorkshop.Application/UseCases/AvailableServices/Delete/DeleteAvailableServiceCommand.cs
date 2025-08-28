using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Delete;

public record DeleteAvailableServiceCommand(Guid Id) : IRequest<Response>;
