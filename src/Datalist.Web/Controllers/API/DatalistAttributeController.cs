using Datalist.Web.Models;
using System.Web.Mvc;

namespace Datalist.Web.Controllers.API
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
