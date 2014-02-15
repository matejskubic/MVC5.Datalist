using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Datalist;

namespace $rootnamespace$.Controllers
{
    public class DatalistController : Controller
    {
        private JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> filters = null)
        {
            datalist.CurrentFilter = filter;
            filter.AdditionalFilters = filters ?? filter.AdditionalFilters;
            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }
        /*
        public JsonResult YourMethod(DatalistFilter filter)
        {
            return GetData(new YourDatalist(), filter);
        }*/
    }
}
