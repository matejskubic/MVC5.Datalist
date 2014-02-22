using System.Web.Mvc;

namespace DatalistSamples.Controllers.API
{
    public class GenericDatalistController : Controller
    {
        #region Constants

        [HttpGet]
        public ActionResult Prefix()
        {
            return View();
        }

        #endregion

        #region Properties

        [HttpGet]
        public ActionResult Columns()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdditionalFilters()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DefaultSortOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DefaultSortColumn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AttributedProperties()
        {
            return View();
        }

        #endregion

        #region Methods

        [HttpGet]
        public ActionResult GetColumnKey()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetColumnHeader()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetColumnCssClass()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetData()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetModels()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FilterById()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FilterBySearchTerm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FilterByAdditionalFilters()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Sort()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormDatalistData()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddId()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAutocomplete()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddColumns()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddAdditionalData()
        {
            return View();
        }

        #endregion
    }
}
