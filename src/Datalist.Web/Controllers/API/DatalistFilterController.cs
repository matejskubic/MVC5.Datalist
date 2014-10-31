using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
{
    public class DatalistFilterController : Controller
    {
        #region Properties


        [HttpGet]
        public ActionResult Id()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Page()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchTerm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SortOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SortColumn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RecordsPerPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdditionalFilters()
        {
            return View();
        }


        #endregion
    }
}
