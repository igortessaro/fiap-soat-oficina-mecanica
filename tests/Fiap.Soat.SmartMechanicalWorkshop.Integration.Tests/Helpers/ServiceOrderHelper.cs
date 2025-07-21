using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Helpers;

public static class ServiceOrderHelper
{
    private static readonly List<ServiceOrder> Orders =
    [
        new ServiceOrder("SO-001", "Description order SO-001", Guid.Empty, Guid.Empty),
        new ServiceOrder("SO-002", "Description order SO-002", Guid.Empty, Guid.Empty),
        new ServiceOrder("SO-003", "Description order SO-003", Guid.Empty, Guid.Empty)
    ];

    public static IReadOnlyList<ServiceOrder> ServiceOrders => Orders;
}
