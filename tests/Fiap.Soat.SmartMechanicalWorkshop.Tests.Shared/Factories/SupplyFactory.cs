using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

[ExcludeFromCodeCoverage]
public static class SupplyFactory
{
    private static readonly List<Supply> SupplyList =
    [
        new("Óleo 5W30", (decimal) 35.00, 0),
        new("Filtro de Óleo", (decimal) 25.00, 0),
        new("Pastilha de Freio", (decimal) 80.00, 0),
        new("Pneu Aro 15", (decimal) 250.00, 0),
        new("Filtro de Ar", (decimal) 40.00, 0),
        new("Correia Dentada", (decimal) 120.00, 0),
        new("Líquido de Arrefecimento", (decimal) 30.00, 0),
        new("Limpador de Para-brisa", (decimal) 20.00, 0),
        new("Desinfetante Automotivo", (decimal) 15.00, 0),
        new("Polidor de Pintura", (decimal) 60.00, 0),
        new("ToDelete", (decimal) 99.99, 0)
    ];

    public static IReadOnlyList<Supply> Supplies => SupplyList;
}
