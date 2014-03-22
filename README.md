MVC.Datalist
============

Autocomplete plus datatables like plugin for MVC projects.

Built on MVC4, Bootstrap and jQuery.UI.

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
Render datalist partial before calling RenderBody in your _Layout.cshtml
```cshtml
@Html.Partial("_Datalist")
```
Include scripts
```html
<script src="~/Scripts/jquery-2.1.0.js" />
<script src="~/Scripts/bootstrap.js" />
<script src="~/Scripts/jquery-ui-1.10.4.js" />
<script src="~/Scripts/Datalist/bind-with-delay.js" />
<script src="~/Scripts/Datalist/bootstrap-paginator.js" />
<script src="~/Scripts/Datalist/datalist.js" />
```
Render your datalist inputs
```
@Html.Datalist("SampleId", new SampleDatalist())
```
Initialize datalist instances
```js
$('.datalist-input').datalist();
```

Examples and API documentation
--
Usage examples and API can be found at [Datalist] (http://non-factors.com/Datalist) website.
