
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.Value == null)
                return new StatusCodeResult((int)result.StatusCode);
            else
                return new ObjectResult(result.Value) { StatusCode = (int?)result.StatusCode };
        }

        public static ActionResult ToActionResult(this Result result)
        {
            return new StatusCodeResult((int)result.StatusCode);
        }
    }
}