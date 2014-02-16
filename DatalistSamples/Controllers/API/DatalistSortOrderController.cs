using System.Web.Mvc;

namespace DatalistSamples.Controllers.API
{
    public class DatalistSortOrderController : Controller
    {
        #region Properties

        [HttpGet]
        public ActionResult Asc()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Desc()
        {
            return View();
        }

        #endregion
    }
}
