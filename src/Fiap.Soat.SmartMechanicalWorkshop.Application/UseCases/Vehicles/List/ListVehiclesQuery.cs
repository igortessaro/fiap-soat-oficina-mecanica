using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;

public record ListVehiclesQuery(int PageNumber, int PageSize) : IRequest<Response<Paginate<Vehicle>>>
{
    public static implicit operator ListVehiclesQuery(PaginatedRequest paginated) => new ListVehiclesQuery(paginated.PageNumber, paginated.PageSize);
    public static implicit operator PaginatedRequest(ListVehiclesQuery paginated) => new PaginatedRequest(paginated.PageNumber, paginated.PageSize);
}
