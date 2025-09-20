using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;

public record ListPeopleQuery(int PageNumber, int PageSize) : IRequest<Response<Paginate<Person>>>
{
    public static implicit operator ListPeopleQuery(PaginatedRequest paginated) => new ListPeopleQuery(paginated.PageNumber, paginated.PageSize);
    public static implicit operator PaginatedRequest(ListPeopleQuery paginated) => new PaginatedRequest(paginated.PageNumber, paginated.PageSize);
}
