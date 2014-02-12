using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Datalist
{
    public static class DatalistExtensions
    {
        public static IHtmlString AutoComplete<TModel>(this HtmlHelper<TModel> html,
            String name, Object value, AbstractDatalist model, Object htmlAttributes = null)
        {
            String autoComplete = FormAutoComplete(html, model, name, htmlAttributes);
            String hiddenInput = FormHiddenInput(html, model, name, value, htmlAttributes);

            return new MvcHtmlString(autoComplete + hiddenInput);
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, AbstractDatalist model, Object htmlAttributes = null)
        {
            return html.AutoComplete(ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model, model, htmlAttributes);
        }
        public static IHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)
        {
            return html.AutoCompleteFor(expression, GetModelFromExpression(expression), htmlAttributes);
        }

        public static IHtmlString Datalist<TModel>(this HtmlHelper<TModel> html,
            String name, Object value, AbstractDatalist model, Object htmlAttributes = null)
        {
            var inputGroup = new TagBuilder("div");
            inputGroup.AddCssClass("input-group");
            inputGroup.InnerHtml = html.AutoComplete(name, value, model, htmlAttributes).ToString() + FormDatalistOpenSpan(model);

            return new MvcHtmlString(inputGroup.ToString());
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, AbstractDatalist model, Object htmlAttributes = null)
        {
            return html.Datalist(ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model, model, htmlAttributes);
        }
        public static IHtmlString DatalistFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)
        {
            return html.DatalistFor(expression, GetModelFromExpression(expression), htmlAttributes);
        }

        private static AbstractDatalist GetModelFromExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            MemberExpression member = expression.Body as MemberExpression;
            PropertyInfo propInfo = member.Member as PropertyInfo;

            DatalistAttribute attr = propInfo.GetCustomAttribute<DatalistAttribute>();
            if (attr == null)
                throw new Exception(String.Format("Property {0} does not have DatalistAttribute specified", propInfo.Name));

            return (AbstractDatalist)Activator.CreateInstance(attr.Type);
        }
        private static String FormAutoComplete<TModel>(HtmlHelper<TModel> html, AbstractDatalist model, String hiddenInput, Object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attributes.Add("data-datalist-filters", String.Join(",", model.AdditionalFilters));
            attributes.Add("data-datalist-records-per-page", model.DefaultRecordsPerPage);
            attributes.Add("data-datalist-sort-column", model.DefaultSortColumn);
            attributes.Add("data-datalist-sort-order", model.DefaultSortOrder);
            attributes.Add("data-datalist-dialog-title", model.DialogTitle);
            attributes.Add("data-datalist-hidden-input", hiddenInput);
            attributes.Add("data-datalist-url", model.DatalistUrl);
            attributes.Add("class", "form-control datalist-input");

            return html.TextBox(AbstractDatalist.Prefix + hiddenInput, null, attributes).ToString();
        }
        private static String FormHiddenInput<TModel>(HtmlHelper<TModel> html, AbstractDatalist model, String name, Object value, Object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            attributes.Add("class", "datalist-hidden-input");

            return html.Hidden(name, value, attributes).ToString();
        }
        private static String FormDatalistOpenSpan(AbstractDatalist model)
        {
            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("datalist-open-span input-group-addon glyphicon glyphicon-search");

            return span.ToString();
        }
    }
}
