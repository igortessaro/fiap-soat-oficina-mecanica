using FluentResults;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared
{
    public class Result<T>
    {
        public FluentResults.Result<T> InnerResult { get; }
        public HttpStatusCode StatusCode { get; }

        public bool IsSuccess => InnerResult.IsSuccess;
        public bool IsFailed => InnerResult.IsFailed;
        public T Value => InnerResult.ValueOrDefault;
        public List<IReason> Reasons => InnerResult.Reasons;

        public Result(FluentResults.Result<T> result, HttpStatusCode statusCode)
        {
            InnerResult = result;
            StatusCode = statusCode;
        }

        // Aqui inferimos T automaticamente
        public static Result<T> Ok(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new(FluentResults.Result.Ok(value), statusCode);

        public static Result<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new(FluentResults.Result.Fail<T>(message), statusCode);

        public static Result<T> Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            => new(FluentResults.Result.Fail<T>(error), statusCode);

        public static Result<T> Fail(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            => new(FluentResults.Result.Fail<T>(errors), statusCode);

        // Conversão implícita para facilitar integração com FluentResults direto
        public static implicit operator FluentResults.Result<T>(Result<T> result)
            => result.InnerResult;
    }

    // Versão não genérica para operações que não retornam valor
    public class Result
    {
        public FluentResults.Result InnerResult { get; }
        public HttpStatusCode StatusCode { get; }

        public bool IsSuccess => InnerResult.IsSuccess;
        public bool IsFailed => InnerResult.IsFailed;
        public List<IReason> Reasons => InnerResult.Reasons;

        public Result(FluentResults.Result result, HttpStatusCode statusCode)
        {
            InnerResult = result;
            StatusCode = statusCode;
        }

        public static Result<object> Ok(object value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(FluentResults.Result.Ok(value), statusCode);

        public static Result Ok(HttpStatusCode statusCode = HttpStatusCode.OK)
            => new(FluentResults.Result.Ok(), statusCode);

        public static Result Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new(FluentResults.Result.Fail(message), statusCode);

        public static Result Fail(Error error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new(FluentResults.Result.Fail(error), statusCode);

        public static Result Fail(List<IError> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new(FluentResults.Result.Fail(errors), statusCode);

        public static implicit operator FluentResults.Result(Result result)
            => result.InnerResult;
    }
}
