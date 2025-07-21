using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
/// Controller for managing vehicles in the Smart Mechanical Workshop API.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Operations related to vehicles.")]
public class VehiclesController(IVehicleService vehicleService) : ControllerBase
{
    /// <summary>
    /// Gets a vehicle by its unique identifier.
    /// </summary>
    /// <param name="id">Vehicle unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The vehicle data.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get a vehicle by ID", Description = "Returns a single vehicle by its unique identifier.")]
    [ProducesResponseType(typeof(VehicleDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await vehicleService.GetOneAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Gets a paginated list of vehicles.
    /// </summary>
    /// <param name="paginatedRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of vehicles.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all vehicles (paginated)", Description = "Returns a paginated list of vehicles.")]
    [ProducesResponseType(typeof(Paginate<VehicleDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await vehicleService.GetAllAsync(paginatedRequest, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Creates a new vehicle.
    /// </summary>
    /// <param name="request">Vehicle creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created vehicle.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new vehicle", Description = "Creates a new vehicle and returns its data.")]
    [ProducesResponseType(typeof(VehicleDto), (int) HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody][Required] CreateNewVehicleRequest request, CancellationToken cancellationToken)
    {
        var result = await vehicleService.CreateAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Deletes a vehicle by its unique identifier.
    /// </summary>
    /// <param name="id">Vehicle unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete a vehicle", Description = "Deletes a vehicle by its unique identifier.")]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await vehicleService.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Updates an existing vehicle.
    /// </summary>
    /// <param name="id">Vehicle unique identifier.</param>
    /// <param name="request">Vehicle update data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated vehicle.</returns>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update a vehicle", Description = "Updates an existing vehicle by its unique identifier.")]
    [ProducesResponseType(typeof(VehicleDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody, Required] UpdateOneVehicleRequest request, CancellationToken cancellationToken)
    {
        UpdateOneVehicleInput updateRequest = new(id, request.LicensePlate, request.ManufactureYear, request.Brand, request.Model, request.PersonId);
        var result = await vehicleService.UpdateAsync(updateRequest, cancellationToken);
        return result.ToActionResult();
    }
}
