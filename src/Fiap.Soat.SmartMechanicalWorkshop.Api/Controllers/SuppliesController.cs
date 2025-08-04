using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
///     Controller for managing supplies.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class SuppliesController(ISupplyService supplyService) : ControllerBase
{
    /// <summary>
    ///     Gets a supply by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the supply.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the supply if found.</returns>
    /// <response code="200">Returns the supply.</response>
    /// <response code="404">If the supply is not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SupplyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await supplyService.GetOneAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Gets a paginated list of all supplies.
    /// </summary>
    /// <param name="paginatedRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a paginated list of supplies.</returns>
    /// <response code="200">Returns the paginated list of supplies.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Paginate<SupplyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await supplyService.GetAllAsync(paginatedRequest, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Creates a new supply.
    /// </summary>
    /// <param name="request">The supply to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the created supply.</returns>
    /// <response code="201">Returns the newly created supply.</response>
    /// <response code="400">If the request is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(SupplyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody][Required] CreateNewSupplyRequest request, CancellationToken cancellationToken)
    {
        var result = await supplyService.CreateAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Deletes a supply by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the supply.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted.</returns>
    /// <response code="204">If the supply was deleted.</response>
    /// <response code="404">If the supply is not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await supplyService.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Updates an existing supply.
    /// </summary>
    /// <param name="id">The unique identifier of the supply.</param>
    /// <param name="request">The updated supply data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns the updated supply.</returns>
    /// <response code="200">Returns the updated supply.</response>
    /// <response code="404">If the supply is not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SupplyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody][Required] UpdateOneSupplyRequest request,
        CancellationToken cancellationToken)
    {
        UpdateOneSupplyInput updateRequest = new(id, request.Name, request.Quantity, request.Price);

        var result = await supplyService.UpdateAsync(updateRequest, cancellationToken);
        return result.ToActionResult();
    }
}
