using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AKWAD_CAMP.Web.Controllers
{
 
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }



        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var scr = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCode >= 400 && statusCode < 500)
            {

                ViewBag.ErrorCode = statusCode;
                ViewBag.ErrorMessage = @$"Page Not Found.
                                    There is no page with this path {scr?.OriginalPath}";
                
            }
            return View("ErrorPage");



        }
    }
}
