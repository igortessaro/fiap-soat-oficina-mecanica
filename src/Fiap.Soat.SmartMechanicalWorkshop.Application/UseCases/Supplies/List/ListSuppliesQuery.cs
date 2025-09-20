using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;

public record ListSuppliesQuery(int PageNumber, int PageSize) : IRequest<Response<Paginate<Supply>>>
{
    public static implicit operator ListSuppliesQuery(PaginatedRequest paginated) => new ListSuppliesQuery(paginated.PageNumber, paginated.PageSize);
    public static implicit operator PaginatedRequest(ListSuppliesQuery paginated) => new PaginatedRequest(paginated.PageNumber, paginated.PageSize);
}
