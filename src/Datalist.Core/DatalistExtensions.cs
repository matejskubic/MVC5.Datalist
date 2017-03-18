using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Datalist
{
    public static class DatalistExtensions
    {
        public static IHtmlString AutoComplete<TModel>(this HtmlHelper<TModel> html,
            String name, MvcDatalist model, Object value = null, Object htmlAttributes = null)
        {
            TagBuilder datalist = CreateDatalist(model, name, htmlAttributes);
            datalist.AddCssClass("datalist-browseless");

            datalist.InnerHtml = CreateDatalistValues(html, model, name, value);
            datalist.InnerHtml += CreateDatalistControl(model, name);

            return new MvcHtmlString(datalist.ToString());
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, MvcDatalist model, Object htmlAttributes = null)
        {
            String name = ExpressionHelper.GetExpressionText(expression);
            TagBuilder datalist = CreateDatalist(model, name, htmlAttributes);
            datalist.AddCssClass("datalist-browseless");

            datalist.InnerHtml = CreateDatalistValues(html, model, expression);
            datalist.InnerHtml += CreateDatalistControl(model, name);

            return new MvcHtmlString(datalist.ToString());
        }

        public static IHtmlString Datalist<TModel>(this HtmlHelper<TModel> html,
            String name, MvcDatalist model, Object value = null, Object htmlAttributes = null)
        {
            TagBuilder datalist = CreateDatalist(model, name, htmlAttributes);

            datalist.InnerHtml = CreateDatalistValues(html, model, name, value);
            datalist.InnerHtml += CreateDatalistControl(model, name);
            datalist.InnerHtml += CreateDatalistBrowse(name);

            return new MvcHtmlString(datalist.ToString());
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, MvcDatalist model, Object htmlAttributes = null)
        {
            String name = ExpressionHelper.GetExpressionText(expression);
            TagBuilder datalist = CreateDatalist(model, name, htmlAttributes);

            datalist.InnerHtml = CreateDatalistValues(html, model, expression);
            datalist.InnerHtml += CreateDatalistControl(model, name);
            datalist.InnerHtml += CreateDatalistBrowse(name);

            return new MvcHtmlString(datalist.ToString());
        }

        private static TagBuilder CreateDatalist(MvcDatalist lookup, String name, Object htmlAttributes)
        {
            IDictionary<String, Object> attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attributes["data-filters"] = String.Join(",", lookup.AdditionalFilters);
            attributes["data-multi"] = lookup.Multi ? "true" : "false";
            attributes["data-order"] = lookup.Filter.Order.ToString();
            attributes["data-page"] = lookup.Filter.Page.ToString();
            attributes["data-rows"] = lookup.Filter.Rows.ToString();
            attributes["data-search"] = lookup.Filter.Search;
            attributes["data-sort"] = lookup.Filter.Sort;
            attributes["data-dialog"] = lookup.Dialog;
            attributes["data-title"] = lookup.Title;
            attributes["data-url"] = lookup.Url;
            attributes["data-for"] = name;

            TagBuilder group = new TagBuilder("div");
            group.MergeAttributes(attributes);
            group.AddCssClass("datalist");

            return group;
        }

        private static String CreateDatalistValues<TModel, TProperty>(HtmlHelper<TModel> html, MvcDatalist datalist, Expression<Func<TModel, TProperty>> expression)
        {
            Object value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
            String name = ExpressionHelper.GetExpressionText(expression);

            if (datalist.Multi)
                return CreateDatalistValues(html, datalist, name, value);

            IDictionary<String, Object> attributes = new Dictionary<String, Object>();
            attributes["class"] = "datalist-value";

            TagBuilder container = new TagBuilder("div");
            container.AddCssClass("datalist-values");
            container.Attributes["data-for"] = name;

            container.InnerHtml = html.HiddenFor(expression, attributes).ToString();

            return container.ToString();
        }
        private static String CreateDatalistValues(HtmlHelper html, MvcDatalist datalist, String name, Object value)
        {
            IDictionary<String, Object> attributes = new Dictionary<String, Object>();
            attributes["class"] = "datalist-value";

            TagBuilder container = new TagBuilder("div");
            container.AddCssClass("datalist-values");
            container.Attributes["data-for"] = name;

            if (datalist.Multi)
            {
                IEnumerable<Object> values = (value as IEnumerable)?.Cast<Object>();
                if (values == null) return container.ToString();

                StringBuilder inputs = new StringBuilder();
                foreach (Object val in values)
                    inputs.Append(html.Hidden(name, val, attributes));

                container.InnerHtml = inputs.ToString();
            }
            else
            {
                container.InnerHtml = html.Hidden(name, value, attributes).ToString();
            }

            return container.ToString();
        }
        private static String CreateDatalistControl(MvcDatalist datalist, String name)
        {
            TagBuilder search = new TagBuilder("input");
            TagBuilder control = new TagBuilder("div");
            TagBuilder loader = new TagBuilder("div");

            loader.AddCssClass("datalist-control-loader");
            control.AddCssClass("datalist-control");
            control.Attributes["data-for"] = name;
            search.AddCssClass("datalist-input");

            control.InnerHtml = search.ToString(TagRenderMode.SelfClosing);
            control.InnerHtml += loader.ToString();

            return control.ToString();
        }
        private static String CreateDatalistBrowse(String name)
        {
            TagBuilder browse = new TagBuilder("div");
            browse.AddCssClass("datalist-browse");
            browse.Attributes["data-for"] = name;

            return browse.ToString();
        }
    }
}
