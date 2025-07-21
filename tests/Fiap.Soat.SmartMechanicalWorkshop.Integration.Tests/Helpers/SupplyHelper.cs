using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Helpers;

public static class SupplyHelper
{
    private static readonly List<Supply> SupplyList =
    [
        new Supply("Óleo 5W30", (decimal)35.00, 0),
        new Supply("Filtro de Óleo", (decimal)25.00, 0),
        new Supply("Pastilha de Freio", (decimal)80.00, 0),
        new Supply("Pneu Aro 15", (decimal)250.00, 0),
        new Supply("Filtro de Ar", (decimal)40.00, 0),
        new Supply("Correia Dentada", (decimal)120.00, 0),
        new Supply("Líquido de Arrefecimento", (decimal)30.00, 0),
        new Supply("Limpador de Para-brisa", (decimal)20.00, 0),
        new Supply("Desinfetante Automotivo", (decimal)15.00, 0),
        new Supply("Polidor de Pintura", (decimal)60.00, 0),
        new Supply("ToDelete", (decimal)99.99, 0)
    ];

    public static IReadOnlyList<Supply> Supplies => SupplyList;
}
