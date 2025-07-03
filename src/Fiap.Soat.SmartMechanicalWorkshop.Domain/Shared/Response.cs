using FluentResults;
using System.Net;
using System.Text.Json.Serialization;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;


public sealed class Response<T>(Result<T> result, HttpStatusCode statusCode)
{
    [JsonIgnore]
    public Result<T> InnerResult { get; } = result;
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; } = statusCode;

    public bool IsSuccess => InnerResult.IsSuccess;
    public bool IsFailed => InnerResult.IsFailed;
    public T Value => InnerResult.ValueOrDefault;
    public List<IReason> Reasons => InnerResult.Reasons;

    // Aqui inferimos T automaticamente
    protected static Response<T> Ok(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(Result.Ok(value), statusCode);

    protected static Response<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(Result.Fail<T>(message), statusCode);

    protected static Response<T> Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(Result.Fail<T>(error), statusCode);

    protected static Response<T> Fail(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(Result.Fail<T>(errors), statusCode);

    // Conversão implícita para facilitar integração com FluentResults direto
    public static implicit operator Result<T>(Response<T> result)
        => result.InnerResult;
}

// Versão não genérica para operações que não retornam valor
public sealed class Response(Result result, HttpStatusCode statusCode)
{
    [JsonIgnore]
    public Result InnerResult { get; } = result;
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; } = statusCode;

    public bool IsSuccess => InnerResult.IsSuccess;
    public bool IsFailed => InnerResult.IsFailed;
    public List<IReason> Reasons => InnerResult.Reasons;

    protected static Response<object> Ok(object value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(Result.Ok(value), statusCode);

    protected static Response Ok(HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(Result.Ok(), statusCode);

    protected static Response Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(Result.Fail(message), statusCode);

    protected static Response Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(Result.Fail(error), statusCode);

    protected static Response Fail(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(Result.Fail(errors), statusCode);

    public static implicit operator Result(Response result)
        => result.InnerResult;
}

public static class ResponseFactory
{
    public static Response Ok(HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(Result.Ok(), statusCode);

    public static Response<T> Ok<T>(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(Result.Ok(value), statusCode);

    public static Response<T> Fail<T>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(Result.Fail<T>(message), statusCode);

    public static Response<T> Fail<T>(Error error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(Result.Fail<T>(error), statusCode);

    public static Response Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    => new(Result.Fail(error), statusCode);

    public static Response<T> Fail<T>(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        => new(Result.Fail<T>(errors), statusCode);
}
