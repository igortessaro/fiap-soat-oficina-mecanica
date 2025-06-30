using FluentResults;
using System.Net;
using System.Text.Json.Serialization;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;


public class Response<T>(FluentResults.Result<T> result, HttpStatusCode statusCode)
{
    [JsonIgnore]
    public FluentResults.Result<T> InnerResult { get; } = result;
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; } = statusCode;

    public bool IsSuccess => InnerResult.IsSuccess;
    public bool IsFailed => InnerResult.IsFailed;
    public T Value => InnerResult.ValueOrDefault;
    public List<IReason> Reasons => InnerResult.Reasons;

    // Aqui inferimos T automaticamente
    public static Response<T> Ok(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(FluentResults.Result.Ok(value), statusCode);

    public static Response<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(FluentResults.Result.Fail<T>(message), statusCode);

    public static Response<T> Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(FluentResults.Result.Fail<T>(error), statusCode);

    public static Response<T> Fail(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(FluentResults.Result.Fail<T>(errors), statusCode);

    // Conversão implícita para facilitar integração com FluentResults direto
    public static implicit operator FluentResults.Result<T>(Response<T> result)
        => result.InnerResult;
}

// Versão não genérica para operações que não retornam valor
public class Response(FluentResults.Result result, HttpStatusCode statusCode)
{
    [JsonIgnore]
    public FluentResults.Result InnerResult { get; } = result;
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; } = statusCode;

    public bool IsSuccess => InnerResult.IsSuccess;
    public bool IsFailed => InnerResult.IsFailed;
    public List<IReason> Reasons => InnerResult.Reasons;

    public static Response<object> Ok(object value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(FluentResults.Result.Ok(value), statusCode);

    public static Response Ok(HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(FluentResults.Result.Ok(), statusCode);

    public static Response Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(FluentResults.Result.Fail(message), statusCode);

    public static Response Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(FluentResults.Result.Fail(error), statusCode);

    public static Response Fail(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(FluentResults.Result.Fail(errors), statusCode);

    public static implicit operator FluentResults.Result(Response result)
        => result.InnerResult;
}
