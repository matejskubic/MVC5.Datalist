using DatalistSamples.Models;
using System.Web.Mvc;

namespace DatalistSamples.Controllers.API
{
    public class DatalistAttributeController : Controller
    {
        #region Properties

        [HttpGet]
        public ActionResult Type()
        {
            return View(new UserModel());
        }

        #endregion
    }
}
