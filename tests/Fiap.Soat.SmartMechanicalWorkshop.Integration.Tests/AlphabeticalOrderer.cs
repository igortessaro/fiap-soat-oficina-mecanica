using Xunit.Abstractions;
using Xunit.Sdk;

namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests;

public sealed class AlphabeticalOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase =>
        testCases.OrderBy(tc => tc.TestMethod.Method.Name);
}
