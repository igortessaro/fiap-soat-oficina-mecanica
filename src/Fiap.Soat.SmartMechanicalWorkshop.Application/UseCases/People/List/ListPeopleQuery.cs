using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;

public sealed class ListPeopleQuery : PaginatedRequest, IRequest<Response<Paginate<PersonDto>>>;
