Installation
------------

Include style sheets

    <link href="~/Content/bootstrap.css" rel="stylesheet" />
    <link href="~/Content/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <link href="~/Content/Datalist/datalist.css" rel="stylesheet" />

Include scripts

    <script src="~/Scripts/jquery-2.1.0.js" />
    <script src="~/Scripts/jquery-ui-1.10.4.js" />
    <script src="~/Scripts/Datalist/datalist.js" />

Render datalist partial before calling RenderBody in your _Layout.cshtml

    @Html.Partial("_Datalist")

Implement data source method in DatalistController

    private JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> filters = null)
    {
        datalist.CurrentFilter = filter;
        filter.AdditionalFilters = filters ?? filter.AdditionalFilters;
        return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
    }
    public JsonResult YourMethod(DatalistFilter filter)
    {
        return GetData(new YourDatalist(), filter);
    }

Render your datalist inputs using one of datalist's html helpers

    @Html.DatalistFor(model => model.SampleId)
    @Html.Datalist("SampleId", new YourDatalist())
    @Html.DatalistFor(model => model.SampleId, new YourDatalist())