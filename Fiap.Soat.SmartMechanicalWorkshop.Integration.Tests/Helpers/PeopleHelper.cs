using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests.Helpers;

public static class PeopleHelper
{
    private static readonly List<Person> People =
    [
        new Person(
            "12345678901",
            "John Doe",
            EPersonType.Client,
            null,
            "john.doe@example.com",
            new Phone("11", "999999999"),
            new Address("Street 1", "City", "State", "12345-678")
        ),
        new Person(
            "98765432100",
            "Jane Smith",
            EPersonType.Employee,
            EEmployeeRole.Mechanic,
            "jane.smith@example.com",
            new Phone("21", "888888888"),
            new Address("Street 2", "OtherCity", "OtherState", "87654-321")
        )
    ];

    public static IReadOnlyList<Person> PeopleList => People;
}
