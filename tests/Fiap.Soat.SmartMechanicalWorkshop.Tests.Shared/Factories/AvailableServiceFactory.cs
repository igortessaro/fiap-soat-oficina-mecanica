using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

public static class AvailableServiceFactory
{
    private static readonly List<AvailableService> Services =
    [
        new AvailableService("Troca de Óleo", (decimal)120.00),
        new AvailableService("Alinhamento e Balanceamento", (decimal)150.00),
        new AvailableService("Revisão Completa", (decimal)350.00),
        new AvailableService("Troca de Pastilha de Freio", (decimal)180.00),
        new AvailableService("Inspeção Veicular", (decimal)90.00),
        new AvailableService("Troca de Filtro de Ar", (decimal)60.00),
        new AvailableService("Troca de Pneu", (decimal)100.00),
        new AvailableService("Polimento", (decimal)200.00),
        new AvailableService("Troca de Correia Dentada", (decimal)400.00),
        new AvailableService("Higienização do Ar Condicionado", (decimal)130.00)
    ];

    public static IReadOnlyList<AvailableService> AvailableServices => Services;
}
