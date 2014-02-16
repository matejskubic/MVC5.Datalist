using System.Web.Mvc;

namespace DatalistSamples.Controllers.API
{
    public class JavascriptController : Controller
    {
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OnAdditionalFilterChange()
        {
            return View();
        }
    }
}
