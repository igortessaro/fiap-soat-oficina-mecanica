using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;

public record CreateAvailableServiceCommand(string Name, decimal Price, IReadOnlyList<ServiceSupplyCommand> Supplies) : IRequest<Response<AvailableServiceDto>>;

public record ServiceSupplyCommand(Guid SupplyId, int Quantity);
