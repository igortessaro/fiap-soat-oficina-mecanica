using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SuppliesController(ISupplyService supplyService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
        {
            Result<SupplyDto> result = await supplyService.GetOneAsync(id, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {
            Result<Paginate<SupplyDto>> result = await supplyService.GetAllAsync(paginatedRequest, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody][Required] CreateNewSupplyRequest request, CancellationToken cancellationToken)
        {
            Result<SupplyDto> result = await supplyService.CreateAsync(request, cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
        {
            Result result = await supplyService.DeleteAsync(id, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody, Required] UpdateOneSupplyRequest request, CancellationToken cancellationToken)
        {
            UpdateOneSupplyInput updateRequest = new()
            {
                Id = id,
                Quantity = request.Quantity,
                Price = request.Price,
                Name = request.Name
            };

            Result<SupplyDto> result = await supplyService.UpdateAsync(updateRequest, cancellationToken);

            return result.ToActionResult();
        }
    }
}
