using Fiap.Soat.SmartMechanicalWorkshop.Api.Models.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Delete;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
///     Controller for managing available services in the Smart Mechanical Workshop API.
/// </summary>
[Route("api/v1/[controller]")]
[Authorize]
[ApiController]
public sealed class AvailableServicesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Gets an available service by its unique identifier.
    /// </summary>
    /// <param name="id">Available service unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The available service data.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get an available service by ID", Description = "Returns a single available service by its unique identifier.")]
    [ProducesResponseType(typeof(AvailableServiceDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAvailableServiceByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Gets a paginated list of available services.
    /// </summary>
    /// <param name="paginatedQuery">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of available services.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all available services (paginated)", Description = "Returns a paginated list of available services.")]
    [ProducesResponseType(typeof(Paginate<AvailableServiceDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery][Required] ListAvailableServicesQuery paginatedQuery, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(paginatedQuery, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Creates a new available service.
    /// </summary>
    /// <param name="request">Available service creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created available service.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new available service", Description = "Creates a new available service and returns its data.")]
    [ProducesResponseType(typeof(AvailableServiceDto), (int) HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody][Required] CreateAvailableServiceRequest request, CancellationToken cancellationToken)
    {
        var supplies = request.Supplies?.Select(x => new CreateServiceSupplyCommand(x.SupplyId, x.Quantity)).ToList() ?? [];
        var command = new CreateAvailableServiceCommand(request.Name, request.Price, supplies);
        var result = await mediator.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Deletes an available service by its unique identifier.
    /// </summary>
    /// <param name="id">Available service unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete an available service", Description = "Deletes an available service by its unique identifier.")]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteAvailableServiceCommand(id), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Updates an existing available service.
    /// </summary>
    /// <param name="id">Available service unique identifier.</param>
    /// <param name="request">Available service update data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated available service.</returns>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update an available service", Description = "Updates an existing available service by its unique identifier.")]
    [ProducesResponseType(typeof(AvailableServiceDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody][Required] UpdateOneAvailableServiceRequest request,
        CancellationToken cancellationToken)
    {
        var supplies = request.Supplies.Select(x => new UpdateServiceSupplyCommand(x.SupplyId, x.Quantity)).ToList();
        UpdateAvailableServiceCommand command = new(id, request.Name, request.Price, supplies);
        var result = await mediator.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}
