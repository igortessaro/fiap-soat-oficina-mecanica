using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;

public sealed class GetAvailableServiceByIdHandler(
    IMapper mapper,
    IAvailableServiceRepository availableServiceRepository) : IRequestHandler<GetAvailableServiceByIdQuery, Response<AvailableServiceDto>>
{
    public async Task<Response<AvailableServiceDto>> Handle(GetAvailableServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await availableServiceRepository.GetAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(entity))
            : ResponseFactory.Fail<AvailableServiceDto>("AvailableService Not Found", HttpStatusCode.NotFound);
    }
}
