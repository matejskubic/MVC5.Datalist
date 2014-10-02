using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
{
    public class JavascriptController : Controller
    {
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FilterChange()
        {
            return View();
        }
    }
}
