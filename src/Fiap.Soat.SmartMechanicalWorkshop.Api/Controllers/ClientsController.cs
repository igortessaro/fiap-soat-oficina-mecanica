using System.ComponentModel.DataAnnotations;
using System.Net;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers;

/// <summary>
/// Controller for managing clients in the Smart Mechanical Workshop API.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
public sealed class ClientsController(IClientService service) : ControllerBase
{
    /// <summary>
    /// Gets a client by its unique identifier.
    /// </summary>
    /// <param name="id">Client unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The client data.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get a client by ID", Description = "Returns a single client by its unique identifier.")]
    [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await service.GetOneAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Gets a paginated list of clients.
    /// </summary>
    /// <param name="paginatedRequest">Pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of clients.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all clients (paginated)", Description = "Returns a paginated list of clients.")]
    [ProducesResponseType(typeof(Paginate<ClientDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(paginatedRequest, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="request">Client creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created client.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new client", Description = "Creates a new client and returns its data.")]
    [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody][Required] CreateClientRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Deletes a client by its unique identifier.
    /// </summary>
    /// <param name="id">Client unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content if deleted.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete a client", Description = "Deletes a client by its unique identifier.")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Updates an existing client.
    /// </summary>
    /// <param name="id">Client unique identifier.</param>
    /// <param name="request">Client update data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated Client.</returns>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update a Client", Description = "Updates an existing Client by its unique identifier.")]
    [ProducesResponseType(typeof(ClientDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute, Required] Guid id, [FromBody, Required] UpdateOneClientRequest request, CancellationToken cancellationToken)
    {
        UpdateOneClientInput input = new(id, request.Fullname, request.Document, request.Email, request.Phone, request.Address);
        var result = await service.UpdateAsync(input, cancellationToken);
        return result.ToActionResult();
    }
}