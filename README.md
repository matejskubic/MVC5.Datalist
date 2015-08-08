Autocomplete plus datatables like controls for ASP.NET MVC 5 projects.

![Semantic](https://img.shields.io/badge/sem-ver-lightgrey.svg?style=plastic)
![Docs](https://img.shields.io/github/release/NonFactors/MVC.Datalist.Web.svg?style=plastic&label=docs)
![NuGet](https://img.shields.io/nuget/v/Datalist.svg?style=plastic)
![Downloads](https://img.shields.io/nuget/dt/Datalist.svg?style=plastic&label=downloads)
![License](https://img.shields.io/badge/license-MIT-green.svg?style=plastic)

Installation
-
Install datalist package from [NuGet] (http://nuget.org)
```
PM> Install-Package Datalist
```
Include style sheets
```html
<link href="~/Content/bootstrap.css" rel="stylesheet" />
<link href="~/Content/themes/base/jquery.ui.all.css" rel="stylesheet" />
<link href="~/Content/Datalist/datalist.css" rel="stylesheet" />
```
Include scripts
```html
<script src="~/Scripts/jquery-2.1.0.js" />
<script src="~/Scripts/jquery-ui-1.10.4.js" />
<script src="~/Scripts/Datalist/datalist.js" />
```
Render datalist partial before calling RenderBody in your _Layout.cshtml
```cshtml
@Html.Partial("_Datalist")
```
Implement data source method in DatalistController
```cs
private JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter)
{
	datalist.CurrentFilter = filter;

	return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
}
public JsonResult Sample(DatalistFilter filter)
{
    return GetData(new SampleDatalist(), filter);
}
```
Render your datalist inputs using one of datalist's html helpers
```
@Html.DatalistFor(model => model.SampleId)
@Html.Datalist("SampleId", new SampleDatalist())
@Html.DatalistFor(model => model.SampleId, new SampleDatalist())
```

Examples and API documentation
--
Usage examples and API can be found at [Datalist] (http://mvc-datalist.azurewebsites.net/) website.

Contribution
-
- Questions or anything conserning MVC.Datalist website/documentation should be reported at [MVC.Datalist.Web](https://github.com/NonFactors/MVC.Datalist.Web).
- Before you start writing a pull request you should discuss it using GitHub issues.
- Bugs, improvements or features should be reported using GitHub issues.
