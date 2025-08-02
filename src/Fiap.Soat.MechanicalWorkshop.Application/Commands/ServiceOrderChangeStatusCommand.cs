using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Commands;

public record ServiceOrderChangeStatusCommand(Guid Id, ServiceOrderStatus Status) : IRequest<Response<ServiceOrderDto>>;
