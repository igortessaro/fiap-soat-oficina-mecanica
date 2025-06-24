namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared
{
    public interface IResult<T> 
    {

        int StatusCode { get; set; }
        T Value { get; set; }
    }

    public interface IResult
    {

        int StatusCode { get; set; }
    }
}