using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AvailableServicesController(IAvailableService service) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.GetOneAsync(id, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {
            var result = await service.GetAllAsync(paginatedRequest, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody][Required] CreateAvailableServiceRequest request, CancellationToken cancellationToken)
        {
            var result = await service.CreateAsync(request, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.DeleteAsync(id, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody, Required] UpdateOneAvailableServiceRequest request, CancellationToken cancellationToken)
        {
            UpdateOneAvailableServiceInput input = new()
            {
                Id = id,
                Name = request.Name,
                Price = request.Price
            };
            var result = await service.UpdateAsync(input, cancellationToken);
            return result.ToActionResult();
        }
    }
}