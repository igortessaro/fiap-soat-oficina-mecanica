using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Presenters;

public static class ActionResultPresenter
{
    public static ActionResult ToActionResult(Response result) => new ObjectResult(result) { StatusCode = (int?) result.StatusCode };
}
