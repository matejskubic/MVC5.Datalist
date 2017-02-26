using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Datalist
{
    public static class DatalistExtensions
    {
        public static IHtmlString AutoComplete<TModel>(this HtmlHelper<TModel> html,
            String name, MvcDatalist model, Object value = null, Object htmlAttributes = null)
        {
            String autoComplete = FormAutoComplete(html, model, name, htmlAttributes);
            String hiddenInput = FormHiddenInput(html, model, name, value);

            return new MvcHtmlString(hiddenInput + autoComplete);
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)
        {
            return html.AutoCompleteFor(expression, GetDatalistFrom(expression), htmlAttributes);
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, MvcDatalist model, Object htmlAttributes = null)
        {
            String name = ExpressionHelper.GetExpressionText(expression);

            String hiddenInput = FormHiddenInputFor(html, model, expression);
            String autoComplete = FormAutoComplete(html, model, name, htmlAttributes);

            return new MvcHtmlString(hiddenInput + autoComplete);
        }

        public static IHtmlString Datalist<TModel>(this HtmlHelper<TModel> html,
            String name, MvcDatalist model, Object value = null, Object htmlAttributes = null)
        {
            TagBuilder datalist = new TagBuilder("div");
            datalist.AddCssClass("datalist-group");
            datalist.InnerHtml = html.AutoComplete(name, model, value, htmlAttributes) + FormDatalistBrowse(name);

            return new MvcHtmlString(datalist.ToString());
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)
        {
            return html.DatalistFor(expression, GetDatalistFrom(expression), htmlAttributes);
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, MvcDatalist model, Object htmlAttributes = null)
        {
            TagBuilder datalist = new TagBuilder("div");
            String name = ExpressionHelper.GetExpressionText(expression);
            datalist.InnerHtml = html.AutoCompleteFor(expression, model, htmlAttributes) + FormDatalistBrowse(name);
            datalist.AddCssClass("datalist-group");

            return new MvcHtmlString(datalist.ToString());
        }

        private static MvcDatalist GetDatalistFrom<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            MemberExpression exp = expression.Body as MemberExpression;
            DatalistAttribute datalist = exp.Member.GetCustomAttribute<DatalistAttribute>();

            if (datalist == null)
                throw new DatalistException($"'{exp.Member.Name}' property does not have a '{typeof(DatalistAttribute).Name}' specified.");

            return (MvcDatalist)Activator.CreateInstance(datalist.Type);
        }
        private static String FormAutoComplete(HtmlHelper html, MvcDatalist model, String hiddenInput, Object htmlAttributes)
        {
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attributes["data-filters"] = String.Join(",", model.AdditionalFilters);
            attributes["class"] = $"{attributes["class"]} datalist-input".Trim();
            attributes["data-multi"] = model.Multi.ToString().ToLower();
            attributes["data-search"] = model.Filter.Search;
            attributes["data-order"] = model.Filter.Order;
            attributes["data-page"] = model.Filter.Page;
            attributes["data-rows"] = model.Filter.Rows;
            attributes["data-sort"] = model.Filter.Sort;
            attributes["data-title"] = model.Title;
            attributes["data-for"] = hiddenInput;
            attributes["data-url"] = model.Url;

            return html.TextBox(hiddenInput + MvcDatalist.Prefix, null, attributes).ToString();
        }

        private static String FormHiddenInputFor<TModel, TProperty>(HtmlHelper<TModel> html, MvcDatalist model, Expression<Func<TModel, TProperty>> expression)
        {
            if (model.Multi)
            {
                Object value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
                String name = ExpressionHelper.GetExpressionText(expression);

                return FormHiddenInput(html, model, name, value);
            }

            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes["class"] = "datalist-hidden-input";

            return html.HiddenFor(expression, attributes).ToString();
        }
        private static String FormHiddenInput(HtmlHelper html, MvcDatalist model, String name, Object value)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes["class"] = "datalist-hidden-input";

            if (model.Multi)
            {
                IEnumerable<Object> values = (value as IEnumerable)?.Cast<Object>();
                if (values == null) return "";

                StringBuilder inputs = new StringBuilder();
                foreach (Object val in values)
                    inputs.Append(html.Hidden(name, val, attributes));

                return inputs.ToString();
            }

            return html.Hidden(name, value, attributes).ToString();
        }

        private static String FormDatalistBrowse(String name)
        {
            TagBuilder browse = new TagBuilder("span");
            browse.AddCssClass("datalist-browse");
            browse.Attributes["data-for"] = name;

            return browse.ToString();
        }
    }
}
