using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;

public record UpdateServiceOrderCommand(Guid Id, string Title, string Description, IReadOnlyList<Guid> ServiceIds) : IRequest<Response<ServiceOrder>>;
