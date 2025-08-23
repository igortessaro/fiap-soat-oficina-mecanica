using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;

public record ListAvailableServicesQuery(int PageNumber, int PageSize) : IRequest<Response<Paginate<AvailableServiceDto>>>
{
    public static implicit operator ListAvailableServicesQuery(PaginatedRequest paginated) => new ListAvailableServicesQuery(paginated.PageNumber, paginated.PageSize);
    public static implicit operator PaginatedRequest(ListAvailableServicesQuery paginated) => new PaginatedRequest(paginated.PageNumber, paginated.PageSize);
}
