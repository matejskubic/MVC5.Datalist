using System.Web.Mvc;

namespace DatalistSamples.Controllers.API
{
    public class DatalistColumnAttributeController : Controller
    {
        #region Properties

        [HttpGet]
        public ActionResult Format()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Position()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Relation()
        {
            return View();
        }

        #endregion
    }
}
