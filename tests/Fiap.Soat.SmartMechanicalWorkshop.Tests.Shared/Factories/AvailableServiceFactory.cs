using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

[ExcludeFromCodeCoverage]
public static class AvailableServiceFactory
{
    private static readonly List<AvailableService> Services =
    [
        new("Troca de Óleo", (decimal) 120.00),
        new("Alinhamento e Balanceamento", (decimal) 150.00),
        new("Revisão Completa", (decimal) 350.00),
        new("Troca de Pastilha de Freio", (decimal) 180.00),
        new("Inspeção Veicular", (decimal) 90.00),
        new("Troca de Filtro de Ar", (decimal) 60.00),
        new("Troca de Pneu", (decimal) 100.00),
        new("Polimento", (decimal) 200.00),
        new("Troca de Correia Dentada", (decimal) 400.00),
        new("Higienização do Ar Condicionado", (decimal) 130.00)
    ];

    public static IReadOnlyList<AvailableService> AvailableServices => Services;
}
