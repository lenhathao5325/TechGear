using Microsoft.AspNetCore.Mvc;

namespace TechGear.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

        [Route("Error/NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
