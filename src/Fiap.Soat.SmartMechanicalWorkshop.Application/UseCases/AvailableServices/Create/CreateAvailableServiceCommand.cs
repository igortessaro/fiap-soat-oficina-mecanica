using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;

public record CreateAvailableServiceCommand(string Name, decimal Price, IReadOnlyList<CreateServiceSupplyCommand> Supplies) : IRequest<Response<AvailableServiceDto>>;

public record CreateServiceSupplyCommand(Guid SupplyId, int Quantity);
