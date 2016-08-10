using Datalist.Tests.Objects;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class GenericDatalistTests : IDisposable
    {
        private Mock<TestDatalistProxy> datalistMock;
        private Dictionary<String, String> row;
        private TestDatalistProxy datalist;

        public GenericDatalistTests()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:7013/", null),
                new HttpResponse(new StringWriter()));

            datalistMock = new Mock<TestDatalistProxy> { CallBase = true };
            row = new Dictionary<String, String>();
            datalist = datalistMock.Object;
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region AttributedProperties

        [Fact]
        public void AttributedProperties_GetsOrderedProperties()
        {
            List<PropertyInfo> actual = datalist.BaseAttributedProperties.ToList();
            List<PropertyInfo> expected = typeof(TestModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GenericDatalist()

        [Fact]
        public void GenericDatalist_CallsGetColumnKey()
        {
            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnKey", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Fact]
        public void GenericDatalist_CallsGetColumnHeader()
        {
            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnHeader", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Fact]
        public void GenericDatalist_CallsGetColumnCssClass()
        {
            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnCssClass", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Fact]
        public void GenericDatalist_AddsColumns()
        {
            DatalistColumns columns = new DatalistColumns();
            foreach (PropertyInfo property in datalist.BaseAttributedProperties)
                columns.Add(new DatalistColumn(datalist.BaseGetColumnKey(property), datalist.BaseGetColumnHeader(property)));

            IEnumerator<DatalistColumn> expected = columns.GetEnumerator();
            IEnumerator<DatalistColumn> actual = datalist.Columns.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Key, actual.Current.Key);
                Assert.Equal(expected.Current.Header, actual.Current.Header);
                Assert.Equal(expected.Current.CssClass, actual.Current.CssClass);
            }
        }

        #endregion

        #region GenericDatalist(UrlHelper url)

        [Fact]
        public void GenericDatalist_Url_CallsGetColumnKey()
        {
            datalistMock = new Mock<TestDatalistProxy>(new UrlHelper(HttpContext.Current.Request.RequestContext));
            datalistMock.CallBase = true;
            datalist = datalistMock.Object;

            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnKey", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Fact]
        public void GenericDatalist_Url_CallsGetColumnHeader()
        {
            datalistMock = new Mock<TestDatalistProxy>(new UrlHelper(HttpContext.Current.Request.RequestContext));
            datalistMock.CallBase = true;
            datalist = datalistMock.Object;

            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnHeader", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Fact]
        public void GenericDatalist_Url_CallsGetColumnCssClass()
        {
            datalistMock = new Mock<TestDatalistProxy>(new UrlHelper(HttpContext.Current.Request.RequestContext));
            datalistMock.CallBase = true;
            datalist = datalistMock.Object;

            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnCssClass", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Fact]
        public void GenericDatalist_Url_AddsColumns()
        {
            datalistMock = new Mock<TestDatalistProxy>(new UrlHelper(HttpContext.Current.Request.RequestContext));
            datalistMock.CallBase = true;
            datalist = datalistMock.Object;

            DatalistColumns columns = new DatalistColumns();
            foreach (PropertyInfo property in datalist.BaseAttributedProperties)
                columns.Add(new DatalistColumn(datalist.BaseGetColumnKey(property), datalist.BaseGetColumnHeader(property)));

            IEnumerator<DatalistColumn> expected = columns.GetEnumerator();
            IEnumerator<DatalistColumn> actual = datalist.Columns.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Key, actual.Current.Key);
                Assert.Equal(expected.Current.Header, actual.Current.Header);
                Assert.Equal(expected.Current.CssClass, actual.Current.CssClass);
            }
        }

        #endregion

        #region GetColumnKey(PropertyInfo property)

        [Fact]
        public void GetColumnKey_NullProperty_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => datalist.BaseGetColumnKey(null));

            Assert.Equal("property", actual.ParamName);
        }

        [Fact]
        public void GetColumnKey_ReturnsPropertyName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");

            String actual = datalist.BaseGetColumnKey(property);
            String expected = property.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnKey_NoRelation_Throws()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseGetColumnKey(property));

            String expected = $"'{property.DeclaringType.Name}.{property.Name}' does not have property named 'None'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnKey_ReturnsRelationKey()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String relation = property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation;

            String expected = $"{property.Name}.{relation}";
            String actual = datalist.BaseGetColumnKey(property);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetColumnHeader(PropertyInfo property)

        [Fact]
        public void GetColumnHeader_NullProperty_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => datalist.BaseGetColumnHeader(null));

            Assert.Equal("property", actual.ParamName);
        }

        [Fact]
        public void GetColumnHeader_ReturnsPropertyName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");

            String actual = datalist.BaseGetColumnHeader(property);
            String expected = property.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsDisplayName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Number");

            String expected = property.GetCustomAttribute<DisplayAttribute>().Name;
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_NoRelation_Throws()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseGetColumnHeader(property));

            String expected = $"'{property.DeclaringType.Name}.{property.Name}' does not have property named 'None'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsRelationName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("SecondRelationModel");

            String expected = property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation;
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsRelationDisplayName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");

            String expected = property.PropertyType.GetProperty("Value").GetCustomAttribute<DisplayAttribute>().Name;
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetColumnCssClass(PropertyInfo property)

        [Fact]
        public void GetColumnCssClass_ReturnsEmptyString()
        {
            Assert.Empty(datalist.BaseGetColumnCssClass(null));
        }

        #endregion

        #region GetData()

        [Fact]
        public void GetData_CallsGetModels()
        {
            datalist.GetData();

            datalistMock.Protected().Verify("GetModels", Times.Once());
        }

        [Fact]
        public void GetData_CallsFilterById()
        {
            datalist.CurrentFilter.Id = "1";

            datalist.GetData();

            datalistMock.Protected().Verify("FilterById", Times.Once(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_DoesNotCallFilterById()
        {
            datalist.CurrentFilter.Id = null;

            datalist.GetData();

            datalistMock.Protected().Verify("FilterById", Times.Never(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_CallsFilterByAdditionalFilters()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");

            datalist.GetData();

            datalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Once(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_DoesNotCallFilterByAdditionalFiltersBecauseEmpty()
        {
            datalist.CurrentFilter.AdditionalFilters.Clear();

            datalist.GetData();

            datalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_DoesNotCallFilterByAdditionalFiltersBecauseFiltersById()
        {
            datalist.CurrentFilter.Id = "1";

            datalist.GetData();
            datalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_CallsFilterBySearchTerm()
        {
            datalist.CurrentFilter.Id = null;

            datalist.GetData();

            datalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_CallsFilterBySearchTermAfterAdditionalFiltered()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");
            datalist.CurrentFilter.Id = null;

            datalist.GetData();

            datalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), datalist.BaseGetModels().Where(model => model.Id == "1"));
        }

        [Fact]
        public void GetData_DoesNotCallFilterBySearchTermBecauseFiltersById()
        {
            datalist.CurrentFilter.Id = "1";

            datalist.GetData();

            datalistMock.Protected().Verify("FilterBySearchTerm", Times.Never(), datalist.BaseGetModels());
        }

        [Fact]
        public void GetData_CallsFormDatalistData()
        {
            datalist.GetData();

            datalistMock.Protected().Verify("FormDatalistData", Times.Once(), datalist.BaseGetModels());
        }

        #endregion

        #region FilterById(IQueryable<T> models)

        [Fact]
        public void FilterById_NoIdProperty_Throws()
        {
            GenericDatalistProxy<NoIdModel> datalist = new GenericDatalistProxy<NoIdModel>();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));

            String expected = $"'{typeof(NoIdModel).Name}' type does not have property named 'Id'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_String()
        {
            datalist.CurrentFilter.Id = "9";

            IEnumerable<TestModel> expected = datalist.BaseGetModels().Where(model => model.Id == datalist.CurrentFilter.Id);
            IEnumerable<TestModel> actual = datalist.BaseFilterById(datalist.BaseGetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_Number()
        {
            List<NumericIdModel> models = new List<NumericIdModel>();
            for (Int32 i = 0; i < 100; i++) models.Add(new NumericIdModel { Id = i });
            GenericDatalistProxy<NumericIdModel> datalist = new GenericDatalistProxy<NumericIdModel>();

            datalist.CurrentFilter.Id = "9.0";

            IEnumerable<NumericIdModel> actual = datalist.BaseFilterById(models.AsQueryable());
            IEnumerable<NumericIdModel> expected = models.Where(model => model.Id == 9);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_EnumId_Throws()
        {
            GenericDatalistProxy<EnumModel> datalist = new GenericDatalistProxy<EnumModel>();

            datalist.CurrentFilter.Id = IdEnum.Id.ToString();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));

            String expected = $"'{typeof(EnumModel).Name}.Id' can not be filtered by using 'Id' value, because it's not a string nor a number.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_NotNumericId_Throws()
        {
            GenericDatalistProxy<NonNumericIdModel> datalist = new GenericDatalistProxy<NonNumericIdModel>();

            datalist.CurrentFilter.Id = "9";

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));

            String expected = $"'{typeof(NonNumericIdModel).Name}.Id' can not be filtered by using '9' value, because it's not a string nor a number.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FilterByAdditionalFilters(IQueryable<T> models)

        [Fact]
        public void FilterByAdditionalFilters_SkipsNullValues()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", null);

            IQueryable<TestModel> actual = datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterByAdditionalFilters_Filters()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", "9");
            datalist.CurrentFilter.AdditionalFilters.Add("Number", 9);
            datalist.CurrentFilter.AdditionalFilters.Add("CreationDate", DateTime.Now.Date.AddDays(9));

            IEnumerable<TestModel> actual = datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels());
            IEnumerable<TestModel> expected = datalist.BaseGetModels().ToArray().Where(model => model.Id == "9" && model.Number == 9 && model.CreationDate == DateTime.Now.Date.AddDays(9));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FilterBySearchTerm(IQueryable<T> models)

        [Fact]
        public void FilterBySearchTerm_SkipsNullTerm()
        {
            datalist.CurrentFilter.SearchTerm = null;

            IQueryable<TestModel> expected = datalist.BaseGetModels();
            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_NoProperty_Throws()
        {
            datalist.CurrentFilter.SearchTerm = "Test";
            datalist.Columns.Add(new DatalistColumn("Test", ""));

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseFilterBySearchTerm(datalist.BaseGetModels()));

            String expected = $"'{typeof(TestModel).Name}' type does not have property named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_FiltersWhiteSpace()
        {
            datalist.CurrentFilter.SearchTerm = " ";

            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_UsesContainsSearch()
        {
            datalist.CurrentFilter.SearchTerm = "1";

            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_NotStringProperties_DoesNotFilter()
        {
            datalist.Columns.Clear();
            datalist.CurrentFilter.SearchTerm = "1";
            datalist.Columns.Add(new DatalistColumn("Number", ""));

            IQueryable<TestModel> expected = datalist.BaseGetModels();
            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(expected.AsQueryable());

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Sort(IQueryable<T> models)

        [Fact]
        public void Sort_ByColumn()
        {
            datalist.CurrentFilter.SortColumn = datalist.BaseAttributedProperties.First().Name;

            IQueryable<TestModel> expected = datalist.BaseGetModels().OrderBy(model => model.Number);
            IQueryable<TestModel> actual = datalist.BaseSort(datalist.BaseGetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_ByDefaultSortColumn()
        {
            datalist.CurrentFilter.SortColumn = null;
            datalist.BaseDefaultSortColumn = datalist.BaseAttributedProperties.First().Name;

            IQueryable<TestModel> expected = datalist.BaseGetModels().OrderBy(model => model.Number);
            IQueryable<TestModel> actual = datalist.BaseSort(datalist.BaseGetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_NoProperty_Throws()
        {
            datalist.CurrentFilter.SortColumn = "Test";

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseSort(datalist.BaseGetModels()));

            String expected = "Datalist does not contain sort column named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_NoDefaultProperty_Throws()
        {
            datalist.BaseDefaultSortColumn = "Test";
            datalist.CurrentFilter.SortColumn = null;

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseSort(datalist.BaseGetModels()));

            String expected = "Datalist does not contain sort column named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_ByFirstColumn()
        {
            datalist.BaseDefaultSortColumn = null;
            datalist.CurrentFilter.SortColumn = null;

            IQueryable<TestModel> actual = datalist.BaseSort(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().OrderBy(model => model.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_EmptyColumns_Throws()
        {
            datalist.Columns.Clear();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseSort(datalist.BaseGetModels()));

            String expected = "Datalist should have at least one column.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDatalistData(IQueryable<T> models)

        [Fact]
        public void FormDatalistData_SetsFilteredRecords()
        {
            Int32 actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).FilteredRecords;
            Int32 expected = datalist.BaseGetModels().Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormDatalistData_FormsColumns()
        {
            DatalistColumns actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).Columns;
            DatalistColumns expected = datalist.Columns;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormDatalistData_FormsRows()
        {
            Int32 expected = datalist.CurrentFilter.RecordsPerPage;
            Int32 actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).Rows.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormDatalistData_FormsRowsUsingModels()
        {
            datalist.CurrentFilter.Page = 3;
            datalist.CurrentFilter.RecordsPerPage = 3;
            datalist.BaseFormDatalistData(datalist.BaseGetModels());

            List<TestModel> models = datalist.BaseGetModels().Skip(9).Take(3).ToList();
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.Is<TestModel>(match => models.Contains(match)));
        }

        [Fact]
        public void FormDatalistData_CallsAddId()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Fact]
        public void FormDatalistData_CallsAddAutocomplete()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddAutocomplete", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Fact]
        public void FormDatalistData_CallsAddColumns()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddColumns", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Fact]
        public void FormDatalistData_CallsAddAdditionalData()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddAdditionalData", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        #endregion

        #region AddId(Dictionary<String, String> row, T model)

        [Fact]
        public void AddId_NoProperty_Throws()
        {
            GenericDatalistProxy<NoIdModel> datalist = new GenericDatalistProxy<NoIdModel>();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseAddId(row, new NoIdModel()));

            String expected = $"'{typeof(NoIdModel).Name}' type does not have property named 'Id'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddId_Key()
        {
            datalist.BaseAddId(row, new TestModel());

            Assert.True(row.ContainsKey(AbstractDatalist.IdKey));
        }

        [Fact]
        public void AddId_Value()
        {
            TestModel model = new TestModel { Id = "Test" };

            datalist.BaseAddId(row, model);

            Assert.True(row.ContainsValue(model.Id));
        }

        [Fact]
        public void AddId_Element()
        {
            datalist.BaseAddId(row, new TestModel());

            Assert.Equal(1, row.Keys.Count);
        }

        #endregion

        #region AddAutocomplete(Dictionary<String, String> row, T model)

        [Fact]
        public void AddAutocomplete_EmptyColumns_Throws()
        {
            datalist.Columns.Clear();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseAddAutocomplete(row, new TestModel()));

            String expected = "Datalist should have at least one column.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAutocomplete_NoProperty_Throws()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("Test", ""));

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseAddAutocomplete(row, new TestModel()));

            String expected = $"'{typeof(TestModel).Name}' type does not have property named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAutocomplete_Key()
        {
            datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.Equal(AbstractDatalist.AcKey, row.First().Key);
        }

        [Fact]
        public void AddAutocomplete_Value()
        {
            TestModel model = new TestModel();
            PropertyInfo firstProperty = model.GetType().GetProperty(datalist.Columns.First().Key);

            datalist.BaseAddAutocomplete(row, model);

            Assert.Equal(firstProperty.GetValue(model).ToString(), row.First().Value);
        }

        [Fact]
        public void AddAutocomplete_RelationValue()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("FirstRelationModel.Value", ""));
            TestModel model = new TestModel { FirstRelationModel = new TestRelationModel { Value = "Test" } };
            PropertyInfo firstProperty = typeof(TestRelationModel).GetProperty("Value");

            datalist.BaseAddAutocomplete(row, model);

            Assert.Equal(firstProperty.GetValue(model.FirstRelationModel).ToString(), row.First().Value);
        }

        [Fact]
        public void AddAutocomplete_Element()
        {
            datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.Equal(1, row.Keys.Count);
        }

        #endregion

        #region AddColumns(Dictionary<String, String> row, T model)

        [Fact]
        public void AddColumns_EmptyColumns_Throws()
        {
            datalist.Columns.Clear();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseAddColumns(null, new TestModel()));

            String expected = "Datalist should have at least one column.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddColumns_NoProperty_Throws()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("Test", ""));

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.BaseAddColumns(row, new TestModel()));

            String expected = $"'{typeof(TestModel).Name}' type does not have property named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddColumns_Keys()
        {
            datalist.BaseAddColumns(row, new TestModel());

            Assert.Equal(datalist.Columns.Keys, row.Keys);
        }

        [Fact]
        public void AddColumns_Values()
        {
            List<String> expected = new List<String>();
            TestModel model = new TestModel { FirstRelationModel = new TestRelationModel { Value = "Test" } };
            foreach (DatalistColumn column in datalist.Columns)
                expected.Add(GetValue(model, column.Key));

            datalist.BaseAddColumns(row, model);

            Assert.Equal(expected, row.Values);
        }

        #endregion

        #region AddAdditionalData(Dictionary<String, String> row, T model)

        [Fact]
        public void AddAdditionalData_DoesNothing()
        {
            datalist.BaseAddAdditionalData(row, new TestModel());

            Assert.Empty(row.Keys);
        }

        #endregion

        #region Testing helpers

        private String GetValue(Object model, String fullPropertyName)
        {
            if (model == null) return "";

            Type type = model.GetType();
            String[] properties = fullPropertyName.Split('.');
            PropertyInfo property = type.GetProperty(properties[0]);

            if (properties.Length > 1)
                return GetValue(property.GetValue(model), String.Join(".", properties.Skip(1)));

            Object value = property.GetValue(model) ?? "";
            DatalistColumnAttribute datalistColumn = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (datalistColumn?.Format != null)
                value = String.Format(datalistColumn.Format, value);

            return value.ToString();
        }

        #endregion
    }
}
