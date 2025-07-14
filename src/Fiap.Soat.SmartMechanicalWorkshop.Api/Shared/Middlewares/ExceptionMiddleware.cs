using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Exceptions;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;
using System.Net;
using System.Text.Json;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest,
            BadHttpRequestException => HttpStatusCode.BadRequest,
            ResourceNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException => HttpStatusCode.NotFound,
            DomainException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var response = new
        {
            statusCode = (int) statusCode,
            message = exception.Message,
            errorType = exception.GetType().Name
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) statusCode;
        return statusCode != HttpStatusCode.BadRequest ?
            context.Response.WriteAsync(JsonSerializer.Serialize(response)) :
            context.Response.WriteAsync(JsonSerializer.Serialize(ResponseFactory.Fail(new Error(exception.Message), statusCode)));
    }
}
