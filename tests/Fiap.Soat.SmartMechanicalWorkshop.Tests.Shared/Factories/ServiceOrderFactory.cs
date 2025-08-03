using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

[ExcludeFromCodeCoverage]
public static class ServiceOrderFactory
{
    private static readonly List<ServiceOrder> Orders =
    [
        new("SO-001", "Description order SO-001", Guid.Empty, Guid.Empty),
        new("SO-002", "Description order SO-002", Guid.Empty, Guid.Empty),
        new("SO-003", "Description order SO-003", Guid.Empty, Guid.Empty)
    ];

    public static IReadOnlyList<ServiceOrder> ServiceOrders => Orders;
}
