using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess && result.Value == null)
                return new NotFoundResult();

            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            if (result.Errors.Count != 0)
                return new NotFoundObjectResult(result.Errors);

            return new BadRequestObjectResult(result.Errors);
        }

        public static ActionResult ToActionResult(this Result result)
        {
      
            if (result.IsSuccess)
                return new OkObjectResult(result);

            if (result.Errors.Count != 0)
                return new NotFoundObjectResult(result.Errors);

            return new BadRequestObjectResult(result.Errors);
        }
    }
}