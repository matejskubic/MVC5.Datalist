using Datalist.Tests.Objects;
using NSubstitute;
using System;
using System.Collections;
using System.Web.Mvc;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistExtensionsTests
    {
        private TestDatalist<TestModel> datalist;
        private HtmlHelper<TestModel> html;

        public DatalistExtensionsTests()
        {
            html = MockHtmlHelper();
            datalist = new TestDatalist<TestModel>();

            datalist.Filter.Page = 2;
            datalist.Filter.Rows = 11;
            datalist.Dialog = "Dialog";
            datalist.Filter.Sort = "First";
            datalist.Filter.Search = "Test";
            datalist.AdditionalFilters.Clear();
            datalist.AdditionalFilters.Add("Add1");
            datalist.AdditionalFilters.Add("Add2");
            datalist.Title = "Dialog datalist title";
            datalist.Url = "http://localhost/Datalist";
            datalist.Filter.Order = DatalistSortOrder.Desc;
        }

        #region AutoComplete<TModel>(this IHtmlHelper<TModel> html, String name, MvcDatalist model, Object value = null, Object htmlAttributes = null)

        [Fact]
        public void AutoComplete_FromModel()
        {
            String actual = html.AutoComplete("Test", datalist, "Value", new { @class = "classes", attribute = "attr" }).ToString();
            String expected =
                "<div class=\"datalist-browseless datalist-group\">" +
                    "<div class=\"datalist-values\" data-for=\"Test\">" +
                        "<input class=\"datalist-value\" id=\"Test\" name=\"Test\" type=\"hidden\" value=\"Value\" />" +
                    "</div>" +
                    "<div attribute=\"attr\" class=\"datalist-control\" data-dialog=\"Dialog\" " +
                        "data-filters=\"Add1,Add2\" data-for=\"Test\" data-multi=\"false\" data-order=\"Desc\" " +
                        "data-page=\"2\" data-rows=\"11\" data-search=\"Test\" data-sort=\"First\" " +
                        "data-title=\"Dialog datalist title\" data-url=\"http://localhost/Datalist\">" +
                        "<input class=\"datalist-input\" />" +
                        "<div class=\"datalist-control-loader\"></div>" +
                    "</div>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AutoCompleteFor<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)

        [Fact]
        public void AutoCompleteFor_NoModel_Throws()
        {
            Exception actual = Assert.Throws<DatalistException>(() => html.AutoCompleteFor(model => model.Id));

            Assert.Equal("'Id' property does not have a 'DatalistAttribute' specified.", actual.Message);
        }

        [Fact]
        public void AutoCompleteFor_Expression()
        {
            String actual = html.AutoCompleteFor(model => model.ParentId, new { @class = "classes", attribute = "attr" }).ToString();
            String expected =
                "<div class=\"datalist-browseless datalist-group\">" +
                    "<div class=\"datalist-values\" data-for=\"ParentId\">" +
                        "<input class=\"datalist-value\" id=\"ParentId\" name=\"ParentId\" type=\"hidden\" value=\"Model&#39;s parent ID\" />" +
                    "</div>" +
                    "<div attribute=\"attr\" class=\"datalist-control\" data-dialog=\"TestDialog\" " +
                        "data-filters=\"Test1,Test2\" data-for=\"ParentId\" data-multi=\"false\" data-order=\"Asc\" " +
                        "data-page=\"3\" data-rows=\"7\" data-search=\"Term\" data-sort=\"Id\" " +
                        "data-title=\"Test datalist title\" data-url=\"http://localhost/Test\">" +
                        "<input class=\"datalist-input\" />" +
                        "<div class=\"datalist-control-loader\"></div>" +
                    "</div>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AutoCompleteFor<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, MvcDatalist model, Object htmlAttributes = null)

        [Fact]
        public void AutoCompleteFor_FromModelExpression()
        {
            String actual = html.AutoCompleteFor(model => model.ParentId, datalist, new { @class = "classes", attribute = "attr" }).ToString();
            String expected =
                "<div class=\"datalist-browseless datalist-group\">" +
                    "<div class=\"datalist-values\" data-for=\"ParentId\">" +
                        "<input class=\"datalist-value\" id=\"ParentId\" name=\"ParentId\" type=\"hidden\" value=\"Model&#39;s parent ID\" />" +
                    "</div>" +
                    "<div attribute=\"attr\" class=\"datalist-control\" data-dialog=\"Dialog\" " +
                        "data-filters=\"Add1,Add2\" data-for=\"ParentId\" data-multi=\"false\" data-order=\"Desc\" " +
                        "data-page=\"2\" data-rows=\"11\" data-search=\"Test\" data-sort=\"First\" " +
                        "data-title=\"Dialog datalist title\" data-url=\"http://localhost/Datalist\">" +
                        "<input class=\"datalist-input\" />" +
                        "<div class=\"datalist-control-loader\"></div>" +
                    "</div>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Datalist<TModel>(this IHtmlHelper<TModel> html, String name, MvcDatalist model, Object value = null, Object htmlAttributes = null)

        [Fact]
        public void Datalist_FromModel()
        {
            String actual = html.Datalist("Test", datalist, "Value", new { @class = "classes", attribute = "attr" }).ToString();
            String expected =
                "<div class=\"datalist-group\">" +
                    "<div class=\"datalist-values\" data-for=\"Test\">" +
                        "<input class=\"datalist-value\" id=\"Test\" name=\"Test\" type=\"hidden\" value=\"Value\" />" +
                    "</div>" +
                    "<div attribute=\"attr\" class=\"datalist-control\" data-dialog=\"Dialog\" " +
                        "data-filters=\"Add1,Add2\" data-for=\"Test\" data-multi=\"false\" data-order=\"Desc\" " +
                        "data-page=\"2\" data-rows=\"11\" data-search=\"Test\" data-sort=\"First\" " +
                        "data-title=\"Dialog datalist title\" data-url=\"http://localhost/Datalist\">" +
                        "<input class=\"datalist-input\" />" +
                        "<div class=\"datalist-control-loader\"></div>" +
                    "</div>" +
                    "<div class=\"datalist-browse\" data-for=\"Test\">" +
                    "</div>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region DatalistFor<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, Object htmlAttributes = null)

        [Fact]
        public void DatalistFor_NoModel_Throws()
        {
            Exception actual = Assert.Throws<DatalistException>(() => html.DatalistFor(model => model.Id));

            Assert.Equal("'Id' property does not have a 'DatalistAttribute' specified.", actual.Message);
        }

        [Fact]
        public void DatalistFor_Expression()
        {
            String actual = html.DatalistFor(model => model.ParentId, new { @class = "classes", attribute = "attr" }).ToString();
            String expected =
                "<div class=\"datalist-group\">" +
                    "<div class=\"datalist-values\" data-for=\"ParentId\">" +
                        "<input class=\"datalist-value\" id=\"ParentId\" name=\"ParentId\" type=\"hidden\" value=\"Model&#39;s parent ID\" />" +
                    "</div>" +
                    "<div attribute=\"attr\" class=\"datalist-control\" data-dialog=\"TestDialog\" " +
                        "data-filters=\"Test1,Test2\" data-for=\"ParentId\" data-multi=\"false\" data-order=\"Asc\" " +
                        "data-page=\"3\" data-rows=\"7\" data-search=\"Term\" data-sort=\"Id\" " +
                        "data-title=\"Test datalist title\" data-url=\"http://localhost/Test\">" +
                        "<input class=\"datalist-input\" />" +
                        "<div class=\"datalist-control-loader\"></div>" +
                    "</div>" +
                    "<div class=\"datalist-browse\" data-for=\"ParentId\">" +
                    "</div>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region DatalistFor<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, MvcDatalist model, Object htmlAttributes = null)

        [Fact]
        public void DatalistFor_ExpressionWithModel()
        {
            String actual = html.DatalistFor(model => model.ParentId, datalist, new { @class = "classes", attribute = "attr" }).ToString();
            String expected =
                "<div class=\"datalist-group\">" +
                    "<div class=\"datalist-values\" data-for=\"ParentId\">" +
                        "<input class=\"datalist-value\" id=\"ParentId\" name=\"ParentId\" type=\"hidden\" value=\"Model&#39;s parent ID\" />" +
                    "</div>" +
                    "<div attribute=\"attr\" class=\"datalist-control\" data-dialog=\"Dialog\" " +
                        "data-filters=\"Add1,Add2\" data-for=\"ParentId\" data-multi=\"false\" data-order=\"Desc\" " +
                        "data-page=\"2\" data-rows=\"11\" data-search=\"Test\" data-sort=\"First\" " +
                        "data-title=\"Dialog datalist title\" data-url=\"http://localhost/Datalist\">" +
                        "<input class=\"datalist-input\" />" +
                        "<div class=\"datalist-control-loader\"></div>" +
                    "</div>" +
                    "<div class=\"datalist-browse\" data-for=\"ParentId\">" +
                    "</div>" +
                "</div>";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Test helpers

        private HtmlHelper<TestModel> MockHtmlHelper()
        {
            ViewDataDictionary<TestModel> viewData = new ViewDataDictionary<TestModel>(new TestModel());
            IViewDataContainer container = Substitute.For<IViewDataContainer>();
            viewData.Model.ParentId = "Model's parent ID";
            container.ViewData = viewData;

            ViewContext viewContext = Substitute.For<ViewContext>();
            viewContext.HttpContext.Items.Returns(new Hashtable());
            viewContext.ViewData = viewData;

            return new HtmlHelper<TestModel>(viewContext, container);
        }

        #endregion
    }
}
