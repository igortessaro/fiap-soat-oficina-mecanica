using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Delete;

public record DeleteVehicleCommand(Guid Id) : IRequest<Response>;
