namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record ServiceOrderExecutionTimeReport(
    int TotalOrders,
    TimeSpan AverageTotalTime,
    TimeSpan AverageAttendanceTime,
    TimeSpan AverageExecutionTime,
    TimeSpan AverageDeliveryTime);
