using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared
{
    public class Result<T> : FluentResults.Result, IResult
    {
        private FluentResults.Result<T> result;
        private HttpStatusCode httpStatusCode;

        public int StatusCode { get; set; }
    }

    public class Result : FluentResults.Result, IResult
    {
        private FluentResults.Result<object> result;
        private HttpStatusCode httpStatusCode;

        public Result(FluentResults.Result<object> result, HttpStatusCode httpStatusCode)
        {
            this.result = result;
            this.httpStatusCode = httpStatusCode;
        }

        public int StatusCode { get; set; }
    }

    public static class ResultExtension
    {
        public static Result Status(this FluentResults.Result result, HttpStatusCode httpStatusCode)
        {
            return new Result(result.ToResult<object>(), httpStatusCode); // Explicitly convert to Result<object>
        }

        public static Result Status<T>(this FluentResults.Result<T> result, HttpStatusCode httpStatusCode)  
        {
            return new Result(result.ToResult<object>(), httpStatusCode); // Explicitly convert to Result<object>
        }
    }
}