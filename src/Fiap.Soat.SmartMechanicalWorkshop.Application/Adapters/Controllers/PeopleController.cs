using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Mappers;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers;

public sealed class PeopleController(IMediator mediator) : IPeopleController
{
    public async Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetPersonByIdQuery(id), cancellationToken);
        var result = ResponseMapper.Map(response, PersonPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await mediator.Send((ListPeopleQuery) paginatedRequest, cancellationToken);
        var result = ResponseMapper.Map(response, PersonPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> CreateAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var phone = new CreatePhoneCommand(request.Phone.AreaCode, request.Phone.Number);
        var address = new CreateAddressCommand(request.Address.Street, request.Address.City, request.Address.State, request.Address.ZipCode);
        CreatePersonCommand command = new(request.Fullname, request.Document, request.PersonType, request.EmployeeRole, request.Email, request.Password, phone, address);
        var response = await mediator.Send(command, cancellationToken);
        var result = ResponseMapper.Map(response, PersonPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }

    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeletePersonCommand(id), cancellationToken);
        return ActionResultPresenter.ToActionResult(response);
    }

    public async Task<IActionResult> UpdateAsync(Guid id, UpdateOnePersonRequest request, CancellationToken cancellationToken)
    {
        var phone = request.Phone != null ? new UpdatePhoneCommand(request.Phone.AreaCode, request.Phone.Number) : null;
        var address = request.Address != null ? new UpdateAddressCommand(request.Address.Street, request.Address.City, request.Address.State, request.Address.ZipCode) : null;
        UpdatePersonCommand input = new(id, request.Fullname, request.Document, request.PersonType, request.EmployeeRole, request.Email, request.Password, phone, address);
        var response = await mediator.Send(input, cancellationToken);
        var result = ResponseMapper.Map(response, PersonPresenter.ToDto);
        return ActionResultPresenter.ToActionResult(result);
    }
}
