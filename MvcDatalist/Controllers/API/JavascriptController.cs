using System.Web.Mvc;

namespace MvcDatalist.Controllers.API
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
