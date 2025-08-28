using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;

public record CreateAvailableServiceCommand(string Name, decimal Price, IReadOnlyList<CreateServiceSupplyCommand> Supplies) : IRequest<Response<AvailableService>>;

public record CreateServiceSupplyCommand(Guid SupplyId, int Quantity);
