using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.List;

public record ListServiceOrdersQuery(int PageNumber, int PageSize, Guid? PersonId) : IRequest<Response<Paginate<ServiceOrderDto>>>;
