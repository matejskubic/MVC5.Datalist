using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
{
    public class DatalistColumnController : Controller
    {
        [HttpGet]
        public ActionResult Key()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Header()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CssClass()
        {
            return View();
        }
    }
}
