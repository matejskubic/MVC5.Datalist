using Datalist.Tests.Objects.Models;
using Datalist.Tests.Objects.Stubs;
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

namespace Datalist.Tests.Unit
{
    [TestFixture]
    public class GenericDatalistTests
    {
        private Mock<TestDatalistStub> datalistMock;
        private TestDatalistStub datalist;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:7013/", null),
                new HttpResponse(new StringWriter()));

            datalistMock = new Mock<TestDatalistStub> { CallBase = true };
            datalist = datalistMock.Object;
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Property: AttributedProperties

        [Test]
        public void AttributedProperties_GetsOrderedProperties()
        {
            List<PropertyInfo> actual = datalist.BaseAttributedProperties.ToList();
            List<PropertyInfo> expected = typeof(TestModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: GenericDatalist()

        [Test]
        public void GenericDatalist_CallsGetColumnKey()
        {
            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnKey", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Test]
        public void GenericDatalist_CallsGetColumnHeader()
        {
            IEnumerable<PropertyInfo> properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnHeader", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Test]
        public void GenericDatalist_CallsGetColumnCssClass()
        {
            IEnumerable<PropertyInfo>  properties = datalist.BaseAttributedProperties;
            Int32 callCount = datalist.BaseAttributedProperties.Count();

            datalistMock.Protected().Verify("GetColumnCssClass", Times.Exactly(callCount), ItExpr.Is<PropertyInfo>(match => properties.Contains(match)));
        }

        [Test]
        public void GenericDatalist_AddsColumns()
        {
            DatalistColumns columns = new DatalistColumns();
            foreach (PropertyInfo property in datalist.BaseAttributedProperties)
                columns.Add(new DatalistColumn(datalist.BaseGetColumnKey(property), datalist.BaseGetColumnHeader(property)));

            IEnumerator<DatalistColumn> expected = columns.GetEnumerator();
            IEnumerator<DatalistColumn> actual = datalist.Columns.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Key, actual.Current.Key);
                Assert.AreEqual(expected.Current.Header, actual.Current.Header);
                Assert.AreEqual(expected.Current.CssClass, actual.Current.CssClass);
            }
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

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Test]
        public void GetColumnCssClass_AlwaysEmptyString()
        {
            Assert.AreEqual(String.Empty, datalist.BaseGetColumnCssClass(null));
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
            GenericDatalistStub<NoIdModel> datalist = new GenericDatalistStub<NoIdModel>();

            Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterById_FiltersStringId()
        {
            datalist.CurrentFilter.Id = "9";

            IEnumerable<TestModel> expected = datalist.BaseGetModels().Where(model => model.Id == datalist.CurrentFilter.Id);
            IEnumerable<TestModel> actual = datalist.BaseFilterById(datalist.BaseGetModels());

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterById_FiltersNumericId()
        {
            List<NumericIdModel> models = new List<NumericIdModel>();
            GenericDatalistStub<NumericIdModel> datalist = new GenericDatalistStub<NumericIdModel>();
            for (Int32 i = 0; i < 100; i++)
                models.Add(new NumericIdModel { Id = i });

            Int32 id = 9;
            datalist.CurrentFilter.Id = id.ToString();

            IEnumerable<NumericIdModel> expected = models.Where(model => model.Id == id);
            IEnumerable<NumericIdModel> actual = datalist.BaseFilterById(models.AsQueryable());

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterById_OnEnumIdPropertyThrows()
        {
            GenericDatalistStub<EnumModel> datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.Id = IdEnum.Id.ToString();

            Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterById_OnNonNumericIdThrows()
        {
            GenericDatalistStub<NonNumericIdModel> datalist = new GenericDatalistStub<NonNumericIdModel>();
            datalist.CurrentFilter.Id = "9";

            Assert.Throws<DatalistException>(() => datalist.BaseFilterById(datalist.BaseGetModels()));
        }

        #endregion

        #region Method: FilterByAdditionalFilters(IQueryable<T> models)

        [Test]
        public void FilterByAdditionalFilters_NotFiltersNulls()
        {
            IQueryable<TestModel> expected = datalist.BaseGetModels();
            datalist.CurrentFilter.AdditionalFilters.Add("Id", null);
            IQueryable<TestModel> actual = datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels());

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
            Int32 numberFilter = 9;
            String stringFilter = "9";
            datalist.CurrentFilter.AdditionalFilters.Add("Id", stringFilter);
            datalist.CurrentFilter.AdditionalFilters.Add("Number", numberFilter);

            IQueryable<TestModel> actual = datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().Where(model => model.Id == stringFilter && model.Number == numberFilter);

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
            GenericDatalistStub<EnumModel> datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.AdditionalFilters.Add("IdEnum", DateTime.Now);

            Assert.Throws<DatalistException>(() => datalist.BaseFilterByAdditionalFilters(datalist.BaseGetModels()));
        }

        #endregion

        #region Method: FilterBySearchTerm(IQueryable<T> models)

        [Test]
        public void FilterBySearchTerm_NotFiltersNull()
        {
            datalist.CurrentFilter.SearchTerm = null;

            IQueryable<TestModel> expected = datalist.BaseGetModels();
            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels());

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterBySearchTerm_OnMissingPropertyThrows()
        {
            datalist.CurrentFilter.SearchTerm = "Test";
            datalist.Columns.Add(new DatalistColumn("TestProperty", String.Empty));

            Assert.Throws<DatalistException>(() => datalist.BaseFilterBySearchTerm(datalist.BaseGetModels()));
        }

        [Test]
        public void FilterBySearchTerm_FiltersWhiteSpace()
        {
            datalist.CurrentFilter.SearchTerm = " ";
            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)));

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterBySearchTerm_UsesContainsSearch()
        {
            datalist.CurrentFilter.SearchTerm = "1";
            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(datalist.CurrentFilter.SearchTerm)));

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterBySearchTerm_NotFiltersNonStringProperties()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("Number", String.Empty));
            datalist.CurrentFilter.SearchTerm = "Test";

            IQueryable<TestModel> expected = datalist.BaseGetModels();
            IQueryable<TestModel> actual = datalist.BaseFilterBySearchTerm(expected.AsQueryable());

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Sort(IQueryable<T> models)

        [Test]
        public void Sort_SortsBySortColumn()
        {
            datalist.CurrentFilter.SortColumn = datalist.BaseAttributedProperties.First().Name;
            IQueryable<TestModel> expected = datalist.BaseGetModels().OrderBy(model => model.Number);
            IQueryable<TestModel> actual = datalist.BaseSort(datalist.BaseGetModels());

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Sort_SortsByDefaultSortColumn()
        {
            datalist.CurrentFilter.SortColumn = null;
            datalist.BaseDefaultSortColumn = datalist.BaseAttributedProperties.First().Name;
            IQueryable<TestModel> expected = datalist.BaseGetModels().OrderBy(model => model.Number);
            IQueryable<TestModel> actual = datalist.BaseSort(datalist.BaseGetModels());

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
            IQueryable<TestModel> actual = datalist.BaseSort(datalist.BaseGetModels());
            IQueryable<TestModel> expected = datalist.BaseGetModels().OrderBy(model => model.Number);

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
            Int32 expected = datalist.BaseGetModels().Count();
            Int32 actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).FilteredRecords;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormDatalistData_FormsColumns()
        {
            DatalistColumns expected = datalist.Columns;
            DatalistColumns actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).Columns;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormDatalistData_FormsRows()
        {
            Int32 expected = datalist.CurrentFilter.RecordsPerPage;
            Int32 actual = datalist.BaseFormDatalistData(datalist.BaseGetModels()).Rows.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FormDatalistData_FormsRowsUsingModels()
        {
            datalist.CurrentFilter.Page = 3;
            datalist.CurrentFilter.RecordsPerPage = 3;
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            List<TestModel> expectedModels = datalist.BaseGetModels().Skip(9).Take(3).ToList();
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.Is<TestModel>(match => expectedModels.Contains(match)));
        }

        [Test]
        public void FormDatalistData_CallsAddId()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void FormDatalistData_CallsAddAutocomplete()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddAutocomplete", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void FormDatalistData_CallsAddColumns()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddColumns", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void FormDatalistData_CallsAddAdditionalData()
        {
            datalist.BaseFormDatalistData(datalist.BaseGetModels());
            Int32 callCount = Math.Min(datalist.CurrentFilter.RecordsPerPage, datalist.BaseGetModels().Count());

            datalistMock.Protected().Verify("AddAdditionalData", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        #endregion

        #region Method: AddId(Dictionary<String, String> row, T model)

        [Test]
        public void AddId_OnMissingPropertyThrows()
        {
            Assert.Throws<DatalistException>(() => new GenericDatalistStub<NoIdModel>().BaseAddId(new Dictionary<String, String>(), new NoIdModel()));
        }

        [Test]
        public void AddId_AddsConstantKey()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
            datalist.BaseAddId(row, new TestModel());

            Assert.IsTrue(row.ContainsKey(AbstractDatalist.IdKey));
        }

        [Test]
        public void AddId_AddsValue()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
            TestModel model = new TestModel { Id = "Test" };

            datalist.BaseAddId(row, model);

            Assert.IsTrue(row.ContainsValue(model.Id));
        }

        [Test]
        public void AddId_AddsOneElement()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
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
            datalist.Columns.Add(new DatalistColumn("TestProperty", String.Empty));

            Assert.Throws<DatalistException>(() => datalist.BaseAddAutocomplete(new Dictionary<String, String>(), new TestModel()));
        }

        [Test]
        public void AddAutocomplete_AddsConstantKey()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
            datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(AbstractDatalist.AcKey, row.First().Key);
        }

        [Test]
        public void AddAutocomplete_AddsValue()
        {
            TestModel model = new TestModel();
            Dictionary<String, String> row = new Dictionary<String, String>();
            PropertyInfo firstProperty = model.GetType().GetProperty(datalist.Columns.First().Key);
            datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model).ToString(), row.First().Value);
        }

        [Test]
        public void AddAutocomplete_AddsRelationValue()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("FirstRelationModel.Value", String.Empty));
            TestModel model = new TestModel { FirstRelationModel = new TestRelationModel { Value = "Test" } };
            PropertyInfo firstProperty = typeof(TestRelationModel).GetProperty("Value");
            Dictionary<String, String> row = new Dictionary<String, String>();
            datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model.FirstRelationModel).ToString(), row.First().Value);
        }

        [Test]
        public void AddAutocomplete_AddsOneElement()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
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
            datalist.Columns.Add(new DatalistColumn("TestProperty", String.Empty));

            Assert.Throws<DatalistException>(() => datalist.BaseAddColumns(new Dictionary<String, String>(), new TestModel()));
        }

        [Test]
        public void AddColumns_AddsKeys()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
            datalist.BaseAddColumns(row, new TestModel());

            CollectionAssert.AreEqual(datalist.Columns.Keys, row.Keys);
        }

        [Test]
        public void AddColumns_AddsValues()
        {
            List<String> expected = new List<String>();
            Dictionary<String, String> row = new Dictionary<String, String>();
            TestModel model = new TestModel { FirstRelationModel = new TestRelationModel { Value = "Test" } };
            foreach (DatalistColumn column in datalist.Columns)
                expected.Add(GetValue(model, column.Key));

            datalist.BaseAddColumns(row, model);

            CollectionAssert.AreEqual(expected, row.Values);
        }

        #endregion

        #region Method: AddAdditionalData(Dictionary<String, String> row, T model)

        [Test]
        public void AddAdditionalData_IsEmptyMethod()
        {
            Dictionary<String, String> row = new Dictionary<String, String>();
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
            PropertyInfo property = type.GetProperty(properties[0]);

            if (properties.Length > 1)
                return GetValue(property.GetValue(model), String.Join(".", properties.Skip(1)));

            value = property.GetValue(model) ?? String.Empty;
            DatalistColumnAttribute datalistColumn = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (datalistColumn != null && datalistColumn.Format != null)
                value = String.Format(datalistColumn.Format, value);

            return value.ToString();
        }

        #endregion
    }
}
