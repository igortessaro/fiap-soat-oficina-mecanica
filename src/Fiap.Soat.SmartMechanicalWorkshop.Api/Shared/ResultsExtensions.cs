
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this Response<T> result)
    {
        if (result.Value == null)
            return new StatusCodeResult((int) result.StatusCode);
        else
            return new ObjectResult(result.Value) { StatusCode = (int?) result.StatusCode };
    }

    public static ActionResult ToActionResult(this Response result)
    {
        return new StatusCodeResult((int) result.StatusCode);
    }

    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess && result.Value == null)
            return new NotFoundResult();

        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        if (result.Errors.Any(e => e.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)))
            return new NotFoundObjectResult(result.Errors);

        if (result.Errors.Count != 0)
            return new BadRequestObjectResult(result.Errors);

        return new BadRequestObjectResult(result.Errors);
    }

    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result);

        if (result.Errors.Any(e => e.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)))
            return new NotFoundObjectResult(result.Errors);

        if (result.Errors.Count != 0)
            return new BadRequestObjectResult(result.Errors);

        return new BadRequestObjectResult(result.Errors);
    }

}