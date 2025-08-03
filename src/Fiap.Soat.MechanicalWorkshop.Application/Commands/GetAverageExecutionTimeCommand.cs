using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Commands;

public record GetAverageExecutionTimeCommand(DateOnly StartDate, DateOnly EndDate) : IRequest<Response<ServiceOrderExecutionTimeReport>>;
