using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace StudyTestDemo.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMsg = "404,您所访问的页面不存在";
                    ViewBag.OriginalPath = statusCodeReExecuteFeature.OriginalPath;
                    ViewBag.OriginalQueryString = statusCodeReExecuteFeature.OriginalQueryString;
                    ViewBag.OriginalPathBase = statusCodeReExecuteFeature.OriginalPathBase;
                    break;
            }

            return View("ErrorPage");
        }
    }
}
