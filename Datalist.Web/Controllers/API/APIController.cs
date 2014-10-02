using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
{
    public class APIController : Controller
    {
        [HttpGet]
        public ActionResult AbstractDatalist()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistAttribute()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistColumnAttribute()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistData()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistExtensions()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistFilter()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenericDatalist()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistSortOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistColumn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DatalistColumns()
        {
            return View();
        }
    }
}
