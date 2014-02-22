using Datalist;
using DatalistTests.Objects.Models;
using DatalistTests.Objects.Stubs;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class GenericDatalistTests
    {
        private Mock<TestDatalistStub> datalistMock;
        private TestDatalistStub datalist;

        [SetUp]
        public virtual void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:7013/", null),
                new HttpResponse(new StringWriter()));

            datalistMock = new Mock<TestDatalistStub>() { CallBase = true };
            datalist = datalistMock.Object;
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Property: AttributedProperties

        [Test]
        public void AttributedProperties_GetsOrderedProperties()
        {
            var actual = datalist.BaseAttributedProperties.ToList();
            var expected = typeof(TestModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: GenericDatalist()

        [Test]
        public void GenericDatalist_AddsColumns()
        {
            var properties = datalist.BaseAttributedProperties;
            var callCount = datalist.BaseAttributedProperties.Count();
            datalistMock.Protected().Verify("GetColumnKey", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
            datalistMock.Protected().Verify("GetColumnHeader", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));

            var columns = new Dictionary<String, String>();
            foreach (PropertyInfo property in datalist.BaseAttributedProperties)
                columns.Add(datalist.BaseGetColumnKey(property), datalist.BaseGetColumnHeader(property));

            CollectionAssert.AreEqual(columns, datalist.Columns);
        }

        #endregion

        #region Method: GetColumnKey(PropertyInfo property)

        [Test]
        public void GetColumnKey_OnNullPropertyThrows()
        {
            Assert.Throws<ArgumentNullException>(() => datalist.BaseGetColumnKey(null));
        }

        [Test]
        public void GetColumnKey_ReturnsPropertyName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");
            String actual = datalist.BaseGetColumnKey(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnKey_OnMissingRelationThrows()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");
            Assert.Throws<DatalistException>(() => datalist.BaseGetColumnKey(property));
        }

        [Test]
        public void GetColumnKey_GetsKeyWithRelation()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String expected = String.Format("{0}.{1}", property.Name, property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation);
            String actual = datalist.BaseGetColumnKey(property);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Test]
        public void GetColumnHeader_OnNullPropertyThrows()
        {
            Assert.Throws<ArgumentNullException>(() => datalist.BaseGetColumnHeader(null));
        }

        [Test]
        public void GetColumnHeader_ReturnsPropertyName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");
            String actual = datalist.BaseGetColumnHeader(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnHeader_ReturnsDisplayName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Number");
            String expected = property.GetCustomAttribute<DisplayAttribute>().Name;
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnHeader_OnMissingRelationThrows()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");
            Assert.Throws<DatalistException>(() => datalist.BaseGetColumnHeader(property));
        }

        [Test]
        public void GetColumnHeader_ReturnsRelationName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("SecondRelationModel");
            String expected = property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation;
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnHeader_ReturnsRelationDisplayName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String expected = property.PropertyType.GetProperty("Value").GetCustomAttribute<DisplayAttribute>().Name;
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetData()

        [Test]
        public void GetData_CallsGetModels()
        {
            datalist.GetData();
            datalistMock.Protected().Verify("GetModels", Times.Once());
        }

        [Test]
        public void GetData_CallsFilterById()
        {
            datalist.CurrentFilter.Id = "1";

            datalist.GetData();
            datalistMock.Protected().Verify("FilterById", Times.Once(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_NotCallsFilterById()
        {
            datalist.CurrentFilter.Id = null;

            datalist.GetData();
            datalistMock.Protected().Verify("FilterById", Times.Never(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_CallsFilterByAdditionalFilters()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");

            datalist.GetData();
            datalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Once(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_NotCallsFilterByAdditionalFiltersBecauseEmpty()
        {
            datalist.CurrentFilter.AdditionalFilters.Clear();

            datalist.GetData();
            datalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_NotCallsFilterByAdditionalFiltersBecauseFiltersById()
        {
            datalist.CurrentFilter.Id = "1";

            datalist.GetData();
            datalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_CallsFilterBySearchTerm()
        {
            datalist.CurrentFilter.Id = null;

            datalist.GetData();
            datalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_CallsFilterBySearchTermAfterAdditionalFiltered()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");
            datalist.CurrentFilter.Id = null;

            datalist.GetData();
            datalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), datalist.BaseGetModels().Where(model => model.Id == "1"));
        }

        [Test]
        public void GetData_NotCallsFilterBySearchTermBecauseFiltersById()
        {
            datalist.CurrentFilter.Id = "1";

            datalist.GetData();
            datalistMock.Protected().Verify("FilterBySearchTerm", Times.Never(), datalist.BaseGetModels());
        }

        [Test]
        public void GetData_CallsFormDatalistData()
        {
            datalist.GetData();
            datalistMock.Protected().Verify("FormDatalistData", Times.Never(), datalist.BaseGetModels());
        }

        #endregion

        #region Method: FilterById(IQueryable<T> models)

        [Test]
        public void FilterById_OnMissingIdPropertyThrows()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterById_FiltersStringId()
        {
            datalist.CurrentFilter.Id = "9";
            var expected = datalist.BaseGetModels().Where(model => model.Id == datalist.CurrentFilter.Id).ToList();
            var actual = datalist.BaseFilterById(datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterById_FiltersNumericId()
        {
            var models = new List<NumericIdModel>();
            var datalist = new GenericDatalistStub<NumericIdModel>();
            for (Int32 i = 0; i < 100; i++)
                models.Add(new NumericIdModel() { Id = i });

            var id = 9;
            datalist.CurrentFilter.Id = id.ToString();

            var expected = models.Where(model => model.Id == id).ToList();
            var actual = datalist.BaseFilterById(models.AsQueryable()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterById_OnEnumIdPropertyThrows()
        {
            var datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.Id = IdEnum.Id.ToString();

            Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterById_OnNonNumericIdThrows()
        {
            var datalist = new GenericDatalistStub<NonNumericIdModel>();
            datalist.CurrentFilter.Id = "9";

            Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));
        }

        #endregion

        #region Method: FilterByAdditionalFilters(IQueryable<T> models)

        [Test]
        public void FilterByAdditionalFilters_NotFiltersNulls()
        {
            var expected = datalist.BaseGetModels().ToList();
            datalist.CurrentFilter.AdditionalFilters.Add("Id", null);
            var actual = datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterByAdditionalFilters_OnMissingPropertyThrows()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("TestProperty", "Test");
            Assert.Throws<DatalistException>(() => datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterByAdditionalFilters_Filters()
        {
            var numberFilter = 9;
            var stringFilter = "9";
            datalist.CurrentFilter.AdditionalFilters.Add("Id", stringFilter);
            datalist.CurrentFilter.AdditionalFilters.Add("Number", numberFilter);
            var actual = datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels()).ToList();
            var expected = datalist.BaseGetModels().Where(model => model.Id == stringFilter && model.Number == numberFilter).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterByAdditionalFilters_OnNotSupportedPropertyThrows()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("CreationDate", DateTime.Now);
            Assert.Throws<DatalistException>(() => datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterByAdditionalFilters_OnEnumThrows()
        {
            var datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.AdditionalFilters.Add("IdEnum", DateTime.Now);

            Assert.Throws<DatalistException>(() => datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels()));
        }

        #endregion

        #region Method: FilterBySearchTerm(IQueryable<T> models)

        [Test]
        public void FilterBySearchTerm_NotFiltersNull()
        {
            datalist.CurrentFilter.SearchTerm = null;
            var expected = datalist.BaseGetModels().ToList();
            var actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterBySearchTerm_OnMissingPropertyThrows()
        {
            datalist.CurrentFilter.SearchTerm = "Test";
            datalist.Columns.Add("TestProperty", String.Empty);
            Assert.Throws<DatalistException>(() => datalist.BaseFilterBySearchTerm(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterBySearchTerm_FiltersWhiteSpace()
        {
            datalist.CurrentFilter.SearchTerm = " ";
            var actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels()).ToList();
            var expected = datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm))).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterBySearchTerm_UsesContainsSearch()
        {
            datalist.CurrentFilter.SearchTerm = "1";
            var actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels()).ToList();
            var expected = datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)));

            CollectionAssert.AreEquivalent(expected.ToList(), actual);
        }

        [Test]
        public void FilterBySearchTerm_NotFiltersNonStringProperties()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add("Number", String.Empty);
            datalist.CurrentFilter.SearchTerm = "Test";

            var expected = datalist.BaseGetModels().ToList();
            var actual = datalist.BaseFilterBySearchTerm(expected.AsQueryable()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Sort(IQueryable<T> models)

        [Test]
        public void Sort_SortsBySortColumn()
        {
            datalist.CurrentFilter.SortColumn = datalist.BaseAttributedProperties.First().Name;
            var expected = datalist.BaseGetModels().OrderBy(model => model.Number).ToList();
            var actual = datalist.BaseSort(datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Sort_SortsByDefaultSortColumn()
        {
            datalist.CurrentFilter.SortColumn = null;
            datalist.BaseDefaultSortColumn = datalist.BaseAttributedProperties.First().Name;
            var expected = datalist.BaseGetModels().OrderBy(model => model.Number).ToList();
            var actual = datalist.BaseSort(datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Sort_MissingPropertyThrows()
        {
            datalist.CurrentFilter.SortColumn = "TestProperty";
            Assert.Throws<DatalistException>(() => datalist.BaseSort(datalist.BaseGetModels()));
        }

        [Test]
        public void Sort_MissingDefaultPropertyThrows()
        {
            datalist.BaseDefaultSortColumn = "TestProperty";
            datalist.CurrentFilter.SortColumn = null;
            Assert.Throws<DatalistException>(() => datalist.BaseSort(datalist.BaseGetModels()));
        }

        [Test]
        public void Sort_SortsByFirstColumn()
        {
            datalist.BaseDefaultSortColumn = null;
            datalist.CurrentFilter.SortColumn = null;
            var actual = datalist.BaseSort(datalist.BaseGetModels()).ToList();
            var expected = datalist.BaseGetModels().OrderBy(model => model.Number).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Sort_OnEmptyColumnsThrows()
        {
            datalist.Columns.Clear();
            Assert.Throws<DatalistException>(() => datalist.BaseSort(datalist.BaseGetModels()));
        }

        #endregion

        #region Method: FormDatalistData(IQueryable<T> models)

        [Test]
        public void FormDatalistData_SetsFilteredRecords()
        {
            var expected = datalist.BaseGetModels().Count();
            var actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).FilteredRecords;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormDatalistData_FormsColumns()
        {
            var expected = datalist.Columns;
            var actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).Columns;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormDatalistData_FormsRows()
        {
            var expected = datalist.CurrentFilter.RecordsPerPage;
            var actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).Rows.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormDatalistData_FormsRowsUsingModels()
        {
            datalist.CurrentFilter.Page = 3;
            datalist.CurrentFilter.RecordsPerPage = 3;
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            var expectedModels = datalist.BaseGetModels().Skip(9).Take(3).ToList();
            var callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.Is<TestModel>(match => expectedModels.Contains(match)));
        }

        [Test]
        public void FormDatalistData_CallsAddId()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            var callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());
            datalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void FormDatalistData_CallsAddAutocomplete()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            var callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());
            datalistMock.Protected().Verify("AddAutocomplete", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void FormDatalistData_CallsAddColumns()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            var callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());
            datalistMock.Protected().Verify("AddColumns", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void FormDatalistData_CallsAddAdditionalData()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            var callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());
            datalistMock.Protected().Verify("AddAdditionalData", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        #endregion

        #region Method: AddId(Dictionary<String, String> row, T model)

        [Test]
        public void AddId_OnMissingPropertyThrows()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            Assert.Throws<DatalistException>(() => datalist.BaseAddId(new Dictionary<String, String>(), new NoIdModel()));
        }

        [Test]
        public void AddId_AddsConstantKey()
        {
            var row = new Dictionary<String, String>();
            datalist.BaseAddId(row, new TestModel());

            Assert.IsTrue(row.ContainsKey(AbstractDatalist.IdKey));
        }

        [Test]
        public void AddId_AddsValue()
        {
            var row = new Dictionary<String, String>();
            var model = new TestModel() { Id = "Test" };

            datalist.BaseAddId(row, model);

            Assert.IsTrue(row.ContainsValue(model.Id));
        }

        [Test]
        public void AddId_AddsOneElement()
        {
            var row = new Dictionary<String, String>();
            datalist.BaseAddId(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }

        #endregion

        #region Method: AddAutocomplete(Dictionary<String, String> row, T model)

        [Test]
        public void AddAutocomplete_OnEmptyColumnsThrows()
        {
            datalist.Columns.Clear();
            Assert.Throws<DatalistException>(() => datalist.BaseAddAutocomplete(null, new TestModel()));
        }

        [Test]
        public void AddAutocomplete_OnMissingPropertyThrows()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add("TestProperty", String.Empty);
            Assert.Throws<DatalistException>(() => datalist.BaseAddAutocomplete(new Dictionary<String, String>(), new TestModel()));
        }

        [Test]
        public void AddAutocomplete_AddsConstantKey()
        {
            var row = new Dictionary<String, String>();
            datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(AbstractDatalist.AcKey, row.First().Key);
        }

        [Test]
        public void AddAutocomplete_AddsValue()
        {
            var model = new TestModel();
            var row = new Dictionary<String, String>();
            var firstProperty = model.GetType().GetProperty(datalist.Columns.First().Key);
            datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model).ToString(), row.First().Value);
        }

        [Test]
        public void AddAutocomplete_AddsRelationValue()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add("FirstRelationModel.Value", String.Empty);
            var model = new TestModel() { FirstRelationModel = new TestRelationModel() { Value = "Test" } };
            var firstProperty = typeof(TestRelationModel).GetProperty("Value");
            var row = new Dictionary<String, String>();
            datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model.FirstRelationModel).ToString(), row.First().Value);
        }

        [Test]
        public void AddAutocomplete_AddsOneElement()
        {
            var row = new Dictionary<String, String>();
            datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }

        #endregion

        #region Method: AddColumns(Dictionary<String, String> row, T model)

        [Test]
        public void AddColumns_OnEmptyColumnsThrows()
        {
            datalist.Columns.Clear();
            Assert.Throws<DatalistException>(() => datalist.BaseAddColumns(null, new TestModel()));
        }

        [Test]
        public void AddColumns_OnMissingPropertyThrows()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add("TestProperty", String.Empty);
            Assert.Throws<DatalistException>(() => datalist.BaseAddColumns(new Dictionary<String, String>(), new TestModel()));
        }

        [Test]
        public void AddColumns_AddsKeys()
        {
            var row = new Dictionary<String, String>();
            datalist.BaseAddColumns(row, new TestModel());

            CollectionAssert.AreEqual(datalist.Columns.Keys, row.Keys);
        }

        [Test]
        public void AddColumns_AddsValues()
        {
            var expected = new List<String>();
            var row = new Dictionary<String, String>();
            var model = new TestModel() { FirstRelationModel = new TestRelationModel() { Value = "Test" } };
            foreach (KeyValuePair<String, String> column in datalist.Columns)
                expected.Add(GetValue(model, column.Key));

            datalist.BaseAddColumns(row, model);

            CollectionAssert.AreEqual(expected, row.Values);
        }

        #endregion

        #region Method: AddAdditionalData(Dictionary<String, String> row, T model)

        [Test]
        public void AddAdditionalData_IsEmptyMethod()
        {
            var row = new Dictionary<String, String>();
            datalist.BaseAddAdditionalData(row, new TestModel());

            CollectionAssert.IsEmpty(row.Keys);
        }

        #endregion

        #region Testing helpers

        private String GetValue(Object model, String fullPropertyName)
        {
            if (model == null) return String.Empty;

            Object value = null;
            Type type = model.GetType();
            String[] properties = fullPropertyName.Split('.');
            var property = type.GetProperty(properties[0]);

            if (properties.Length > 1)
                return GetValue(property.GetValue(model), String.Join(".", properties.Skip(1)));

            value = property.GetValue(model) ?? String.Empty;
            var datalistColumn = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (datalistColumn != null && datalistColumn.Format != null)
                value = String.Format(datalistColumn.Format, value);

            return value.ToString();
        }

        #endregion
    }
}
