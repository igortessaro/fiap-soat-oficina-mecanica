using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;

public record UpdateAvailableServiceCommand(Guid Id, string Name, decimal? Price, IReadOnlyList<UpdateServiceSupplyCommand> Supplies) : IRequest<Response<AvailableService>>;

public record UpdateServiceSupplyCommand(Guid SupplyId, int Quantity);
