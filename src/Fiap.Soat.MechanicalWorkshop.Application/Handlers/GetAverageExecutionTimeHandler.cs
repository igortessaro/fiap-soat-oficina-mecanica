using Fiap.Soat.MechanicalWorkshop.Application.Commands;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class GetAverageExecutionTimeHandler(IServiceOrderEventRepository repository, IMemoryCache memoryCache)
    : IRequestHandler<GetAverageExecutionTimeCommand, Response<ServiceOrderExecutionTimeReport>>
{
    public async Task<Response<ServiceOrderExecutionTimeReport>> Handle(GetAverageExecutionTimeCommand request, CancellationToken cancellationToken)
    {
        var cachedValue = await memoryCache.GetOrCreateAsync(
            request.ToString(),
            async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);
                return await repository.GetAverageExecutionTimesAsync(request.StartDate.ToDateTime(TimeOnly.MinValue),
                    request.EndDate.ToDateTime(TimeOnly.MaxValue),
                    cancellationToken);
            });

        return ResponseFactory.Ok(cachedValue!);
    }
}
