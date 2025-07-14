using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Commands;

public record ServiceOrderChangeStatusCommand(Guid Id, EServiceOrderStatus Status) : IRequest<Response<ServiceOrderDto>>;
