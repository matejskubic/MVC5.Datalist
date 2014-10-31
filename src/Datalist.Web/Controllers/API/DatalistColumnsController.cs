using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
{
    public class DatalistColumnsController : Controller
    {
        [HttpGet]
        public ActionResult Keys()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Remove()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Clear()
        {
            return View();
        }
    }
}
