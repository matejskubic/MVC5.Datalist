using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
{
    public class DatalistDataController : Controller
    {
        #region Properties

        [HttpGet]
        public ActionResult Columns()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Rows()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FilteredRecords()
        {
            return View();
        }

        #endregion
    }
}
