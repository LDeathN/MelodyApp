using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult NotFoundPage()
    {
        return View("~/Views/Shared/Error404.cshtml");
    }

    [Route("Error/500")]
    [Route("Error/ServerError")]
    public IActionResult ServerError()
    {
        return View("~/Views/Shared/Error500.cshtml");
    }
}
