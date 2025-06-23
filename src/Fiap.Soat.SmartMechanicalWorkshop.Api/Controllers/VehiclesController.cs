using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VehiclesController(IVehicleService vehicleService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOneAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
        {
            FluentResults.Result<VehicleDto> result = await vehicleService.GetOneAsync(id, cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery][Required] PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {
            FluentResults.Result<Paginate<VehicleDto>> result = await vehicleService.GetAllAsync(paginatedRequest, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody][Required] CreateNewVehicleRequest request, CancellationToken cancellationToken)
        {
            FluentResults.Result<VehicleDto> result = await vehicleService.CreateAsync(request, cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute][Required] Guid id, CancellationToken cancellationToken)
        {
            FluentResults.Result result = await vehicleService.DeleteAsync(id, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute][Required] Guid id, [FromBody, Required] UpdateOneVehicleRequest request, CancellationToken cancellationToken)
        {
            UpdateOneVehicleInput updateRequest = new()
            {
                Id = id,
                ClientId = request.ClientId,
                Model = request.Model,
                Brand = request.Brand,
                LicensePlate = request.LicensePlate,
                ManufactureYear = request.ManufactureYear
            };

            FluentResults.Result<VehicleDto> result = await vehicleService.UpdateAsync(updateRequest, cancellationToken);

            return result.ToActionResult();
        }
    }
}
