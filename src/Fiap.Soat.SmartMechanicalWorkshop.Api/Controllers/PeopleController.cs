using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
///     Controller for managing people in the Smart Mechanical Workshop API.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public sealed class PeopleController(IPersonService service) : ControllerBase
{
    /// <summary>
    ///     Gets a person by its unique identifier.
    /// </summary>
    /// <param name="id">Person unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person data.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get a person by ID", Description = "Returns a single person by its unique identifier.")]
    [ProducesResponseType(typeof(PersonDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOneAsync([FromRoute] [Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await service.GetOneAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Gets a paginated list of people.
    /// </summary>
    /// <param name="paginatedRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of people.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all people (paginated)", Description = "Returns a paginated list of people.")]
    [ProducesResponseType(typeof(Paginate<PersonDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery] [Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(paginatedRequest, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Creates a new person.
    /// </summary>
    /// <param name="request">Person creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created person.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new person", Description = "Creates a new person and returns its data.")]
    [ProducesResponseType(typeof(PersonDto), (int) HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] [Required] CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Deletes a person by its unique identifier.
    /// </summary>
    /// <param name="id">Person unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete a person", Description = "Deletes a person by its unique identifier.")]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] [Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Updates an existing person.
    /// </summary>
    /// <param name="id">Person unique identifier.</param>
    /// <param name="request">Person update data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated Person.</returns>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update a Person", Description = "Updates an existing Person by its unique identifier.")]
    [ProducesResponseType(typeof(PersonDto), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] [Required] Guid id, [FromBody] [Required] UpdateOnePersonRequest request,
        CancellationToken cancellationToken)
    {
        UpdateOnePersonInput input = new(id, request.Fullname, request.Document, request.PersonType, request.EmployeeRole, request.Email, request.Password,
            request.Phone, request.Address);
        var result = await service.UpdateAsync(input, cancellationToken);
        return result.ToActionResult();
    }
}
