namespace Fiap.Soat.SmartMechanicalWorkshop.Integration.Tests;

public sealed class Response<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; } = default!;
    public List<Reason> Reasons { get; set; } = [];
}

public sealed class Reason
{
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
}
