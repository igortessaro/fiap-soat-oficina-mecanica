using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;

public record UpdateAvailableServiceCommand(Guid Id, string Name, decimal? Price, IReadOnlyList<UpdateServiceSupplyCommand> Supplies) : IRequest<Response<AvailableServiceDto>>;

public record UpdateServiceSupplyCommand(Guid SupplyId, int Quantity);
