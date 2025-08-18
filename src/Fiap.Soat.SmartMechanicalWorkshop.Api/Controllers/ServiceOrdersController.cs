using Fiap.Soat.SmartMechanicalWorkshop.Application.Commands;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Notifications;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
///     Controller for managing service orders in the Smart Mechanical Workshop API.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public sealed class ServiceOrdersController(IServiceOrderService service, IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Gets a service order by its unique identifier.
    /// </summary>
    /// <param name="id">Service orders unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The service orders data.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get a service orders by id", Description = "Returns a single service orders by its unique identifier.")]
    [ProducesResponseType(typeof(ServiceOrderDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await service.GetOneAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Gets a paginated list of service orders.
    /// </summary>
    /// <param name="personId">Person Id</param>
    /// <param name="paginatedRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of service orders.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all service orders (paginated)", Description = "Returns a paginated list of service orders.")]
    [ProducesResponseType(typeof(Paginate<ServiceOrderDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery][Required] PaginatedRequest paginatedRequest,
        [FromQuery] Guid? personId,
        CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(personId, paginatedRequest, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Gets the average execution time of service orders.
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The average execution time</returns>
    [HttpGet("average-execution-time")]
    [SwaggerOperation(
        Summary = "Get average execution time of service orders",
        Description = "Returns the average execution time for all service orders."
    )]
    [ProducesResponseType(typeof(Response<ServiceOrderExecutionTimeReport>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAverageExecutionTime([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAverageExecutionTimeCommand(startDate, endDate), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Creates a new service order.
    /// </summary>
    /// <param name="request">Service order creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created service order.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new service order", Description = "Creates a new service order and returns its data.")]
    [ProducesResponseType(typeof(ServiceOrderDto), (int) HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody][Required] CreateServiceOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        if (result.IsSuccess) await mediator.Publish(new ServiceOrderChangeStatusNotification(result.Data.Id, result.Data), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Deletes a service order by its unique identifier.
    /// </summary>
    /// <param name="id">Service order unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete a service order", Description = "Deletes a service order by its unique identifier.")]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Updates an existing service order.
    /// </summary>
    /// <param name="id">Service order unique identifier.</param>
    /// <param name="request">Service order update data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated service order.</returns>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update a service order", Description = "Updates an existing service order by its unique identifier.")]
    [ProducesResponseType(typeof(ServiceOrderDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody][Required] UpdateOneServiceOrderRequest request,
        CancellationToken cancellationToken)
    {
        UpdateOneServiceOrderInput input = new(id, request.Title, request.Description, request.ServiceIds);
        var result = await service.UpdateAsync(input, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Partially updates the status of a service order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the service order to update.</param>
    /// <param name="request">The patch request containing the new status.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated service order if successful, otherwise an error response.</returns>
    [HttpPatch("{id:guid}")]
    [SwaggerOperation(
        Summary = "Patch a service order",
        Description = "Updates an existing service order by its unique identifier with partial data."
    )]
    [ProducesResponseType(typeof(ServiceOrderDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PatchAsync([FromRoute][Required] Guid id, [FromBody][Required] PatchServiceOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ServiceOrderChangeStatusCommand(id, request.Status), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPatch("{id:guid}/quote/{quoteId:guid}/{status}")]
    [SwaggerOperation(
        Summary = "Approve a quote for a service order",
        Description = "Approves a quote for a service order."
    )]
    [ProducesResponseType(typeof(ServiceOrderDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PatchQuoteAsync(
        [FromRoute][Required] Guid id,
        [FromRoute][Required] Guid quoteId,
        [FromRoute][Required] QuoteStatus status,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new QuoteChangeStatusCommand(quoteId, status, id), cancellationToken);
        return result.ToActionResult();
    }
}
