using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Datalist
{
    public static class DatalistExtensions
    {
        public static IHtmlString AutoComplete<TModel>(this HtmlHelper<TModel> html,
            String name, Object value, AbstractDatalist model, Object htmlAttributes = null)
        {
            String autoComplete = FormAutoComplete(html, model, name, htmlAttributes);
            String hiddenInput = FormHiddenInput(html, name, value);

            return new MvcHtmlString(autoComplete + hiddenInput);
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)
        {
            return html.AutoCompleteFor(expression, GetModelFromExpression(expression), htmlAttributes);
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, AbstractDatalist model, Object htmlAttributes = null)
        {
            String name = ExpressionHelper.GetExpressionText(expression);

            String autoComplete = FormAutoComplete(html, model, name, htmlAttributes);
            String hiddenInput = FormHiddenInputFor(html, expression);

            return new MvcHtmlString(autoComplete + hiddenInput);
        }

        public static IHtmlString Datalist<TModel>(this HtmlHelper<TModel> html,
            String name, Object value, AbstractDatalist model, Object htmlAttributes = null)
        {
            TagBuilder inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("input-group");
            inputGroup.InnerHtml = html.AutoComplete(name, value, model, htmlAttributes) + FormDatalistOpenSpan();

            return new MvcHtmlString(inputGroup.ToString());
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)
        {
            return html.DatalistFor(expression, GetModelFromExpression(expression), htmlAttributes);
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, AbstractDatalist model, Object htmlAttributes = null)
        {
            TagBuilder inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("input-group");
            inputGroup.InnerHtml = html.AutoCompleteFor(expression, model, htmlAttributes) + FormDatalistOpenSpan();

            return new MvcHtmlString(inputGroup.ToString());
        }

        private static AbstractDatalist GetModelFromExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            MemberExpression exp = expression.Body as MemberExpression;
            DatalistAttribute datalist = exp.Member.GetCustomAttribute<DatalistAttribute>();

            if (datalist == null)
                throw new DatalistException($"'{exp.Member.Name}' property does not have a '{typeof(DatalistAttribute).Name}' specified.");

            return (AbstractDatalist)Activator.CreateInstance(datalist.Type);
        }
        private static String FormAutoComplete(HtmlHelper html, AbstractDatalist model, String hiddenInput, Object htmlAttributes)
        {
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attributes.Add("data-datalist-filters", String.Join(",", model.AdditionalFilters));
            attributes["class"] = $"{attributes["class"]} form-control datalist-input".Trim();
            attributes.Add("data-datalist-for", TagBuilder.CreateSanitizedId(hiddenInput));
            attributes.Add("data-datalist-sort-column", model.Filter.SortColumn);
            attributes.Add("data-datalist-sort-order", model.Filter.SortOrder);
            attributes.Add("data-datalist-search", model.Filter.Search);
            attributes.Add("data-datalist-page", model.Filter.Page);
            attributes.Add("data-datalist-rows", model.Filter.Rows);
            attributes.Add("data-datalist-title", model.Title);
            attributes.Add("data-datalist-url", model.Url);

            return html.TextBox(hiddenInput + AbstractDatalist.Prefix, null, attributes).ToString();
        }

        private static String FormHiddenInputFor<TModel, TProperty>(HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes.Add("class", "datalist-hidden-input");

            return html.HiddenFor(expression, attributes).ToString();
        }
        private static String FormHiddenInput(HtmlHelper html, String name, Object value)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes.Add("class", "datalist-hidden-input");

            return html.Hidden(name, value, attributes).ToString();
        }

        private static String FormDatalistOpenSpan()
        {
            TagBuilder outerSpan = new TagBuilder("span");
            TagBuilder innerSpan = new TagBuilder("span");

            outerSpan.AddCssClass("datalist-open-span input-group-addon");
            innerSpan.AddCssClass("datalist-open-icon glyphicon");
            outerSpan.InnerHtml = innerSpan.ToString();

            return outerSpan.ToString();
        }
    }
}
