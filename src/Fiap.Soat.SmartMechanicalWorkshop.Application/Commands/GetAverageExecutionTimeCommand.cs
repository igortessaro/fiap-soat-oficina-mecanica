using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Commands;

public record GetAverageExecutionTimeCommand(DateOnly StartDate, DateOnly EndDate) : IRequest<Response<ServiceOrderExecutionTimeReport>>;
