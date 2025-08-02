using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

public class ServiceOrderEventRepository(AppDbContext appDbContext) : Repository<ServiceOrderEvent>(appDbContext), IServiceOrderEventRepository
{
    public async Task<TimeSpan> GetAverageExecutionTime(CancellationToken cancellationToken)
    {
        await using var connection = base.GetDbConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = @"SELECT
            AVG(TIMESTAMPDIFF(SECOND, MinCreatedAt, MaxCreatedAt)) * 10000000 AS AverageTicks
        FROM (
            SELECT
                service_order_id,
                MIN(created_at) AS MinCreatedAt,
                MAX(created_at) AS MaxCreatedAt
            FROM service_order_events
            WHERE Status IN ('InProgress', 'Delivered')
            GROUP BY service_order_id
            HAVING
                COUNT(CASE WHEN Status = 'InProgress' THEN 1 END) > 0 AND
                COUNT(CASE WHEN Status = 'Delivered' THEN 1 END) > 0
        ) AS TimeDiffs;
        ";
        object? result = await command.ExecuteScalarAsync(cancellationToken);
        decimal decimalResult = Math.Round((decimal) (result ?? decimal.Zero));

        return TimeSpan.FromTicks((long) decimalResult);
    }
}
