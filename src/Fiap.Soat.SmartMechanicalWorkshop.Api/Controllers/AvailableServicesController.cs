using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
/// Controller for managing available services in the Smart Mechanical Workshop API.
/// </summary>
[Route("api/v1/[controller]")]
[Authorize]
[ApiController]
public class AvailableServicesController(IAvailableService service) : ControllerBase
{
    /// <summary>
    /// Gets an available service by its unique identifier.
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
        var result = await service.GetOneAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Gets a paginated list of available services.
    /// </summary>
    /// <param name="paginatedRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of available services.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all available services (paginated)", Description = "Returns a paginated list of available services.")]
    [ProducesResponseType(typeof(Paginate<AvailableServiceDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(paginatedRequest, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Creates a new available service.
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
        var result = await service.CreateAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Deletes an available service by its unique identifier.
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
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Updates an existing available service.
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
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody, Required] UpdateOneAvailableServiceRequest request,
        CancellationToken cancellationToken)
    {
        UpdateOneAvailableServiceInput input = new(id, request.Name, request.Price, request.SuppliesIds);
        var result = await service.UpdateAsync(input, cancellationToken);
        return result.ToActionResult();
    }
}
