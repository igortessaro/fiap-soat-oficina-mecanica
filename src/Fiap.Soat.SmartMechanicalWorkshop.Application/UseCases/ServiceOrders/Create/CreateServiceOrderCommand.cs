using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Create;

public record CreateServiceOrderCommand(Guid ClientId, Guid VehicleId, IReadOnlyList<Guid> ServiceIds, string Title, string Description)
    : IRequest<Response<ServiceOrderDto>>;
