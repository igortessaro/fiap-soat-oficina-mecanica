using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record ServiceOrderExecutionTimeReport(
    int TotalOrders,
    TimeSpan AverageTotalTime,
    TimeSpan AverageAttendanceTime,
    TimeSpan AverageExecutionTime,
    TimeSpan AverageDeliveryTime);
