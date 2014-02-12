using Datalist;
using MvcDatalist.Datalists;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcDatalist.Controllers
{
    public class DatalistController : Controller
    {
        private JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter, IDictionary<String, Object> filters = null)
        {
            datalist.CurrentFilter = filter;
            filter.AdditionalFilters = filters ?? filter.AdditionalFilters;
            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Default(DatalistFilter filter)
        {
            return GetData(new DefaultDatalist(), filter);
        }
        public JsonResult Data(DatalistFilter filter)
        {
            return GetData(new DatalistDataDatalist(), filter);
        }
        public JsonResult DifferentUrlExample(DatalistFilter filter, String AdditionalFilterId)
        {
            var additionalFilters = new Dictionary<String, Object>();
            additionalFilters.Add("Id", AdditionalFilterId);

            return GetData(new ExampleDatalist(), filter, additionalFilters);
        }

        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Installation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Examples()
        {
            return View();
        }

        [HttpGet]
        public ActionResult API()
        {
            return View();
        }
    }
}
