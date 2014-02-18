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
        private Mock<TestDatalistStub> DatalistMock;
        private TestDatalistStub Datalist;

        [SetUp]
        public virtual void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:7013/", null),
                new HttpResponse(new StringWriter()));

            DatalistMock = new Mock<TestDatalistStub>() { CallBase = true };
            Datalist = DatalistMock.Object;
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Property: AttributedProperties

        [Test]
        public void Attributed()
        {
            var actual = Datalist.BaseAttributedProperties.ToList();
            var expected = typeof(TestModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: GenericDatalist()
        
        #endregion

        #region Method: GetColumnKey(PropertyInfo property)

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullProperty()
        {
            Datalist.BaseGetColumnKey(null);
        }

        [Test]
        public void NoAttribute()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");
            String actual = Datalist.BaseGetColumnKey(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NotExistingRelation()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");
            Datalist.BaseGetColumnKey(property);
        }

        [Test]
        public void RelationProperty()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String expected = String.Format("{0}.{1}", property.Name, property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation);
            String actual = Datalist.BaseGetColumnKey(property);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetColumnHeader_NullProperty()
        {
            Datalist.BaseGetColumnHeader(null);
        }

        [Test]
        public void GetColumnHeader_NoAttribute()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");
            String actual = Datalist.BaseGetColumnHeader(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnHeader_Display()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Number");
            String expected = property.GetCustomAttribute<DisplayAttribute>().Name;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void GetColumnHeader_NotExistingRelation()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");
            Datalist.BaseGetColumnHeader(property);
        }

        [Test]
        public void GetColumnHeader_RelationProperty()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("SecondRelationModel");
            String expected = property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnHeader_RelationDisplay()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String expected = property.PropertyType.GetProperty("Value").GetCustomAttribute<DisplayAttribute>().Name;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetData()

        [Test]
        public void GetModelsCalled()
        {
            Datalist.GetData();
            DatalistMock.Protected().Verify("GetModels", Times.Once());
        }

        [Test]
        public void FilterByIdCalled()
        {
            Datalist.CurrentFilter.Id = "1";

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterById", Times.Once(), Datalist.BaseGetModels());
        }

        [Test]
        public void FilterByIdNotCalled()
        {
            Datalist.CurrentFilter.Id = null;

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterById", Times.Never(), Datalist.BaseGetModels());
        }

        [Test]
        public void FilterByAdditionalFiltersCalled()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Once(), Datalist.BaseGetModels());
        }

        [Test]
        public void FilterByAdditionalFiltersNotCalledBecauseEmpty()
        {
            Datalist.CurrentFilter.AdditionalFilters.Clear();

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), Datalist.BaseGetModels());
        }

        [Test]
        public void FilterByAdditionalFiltersNotCalledBecauseIdFiltered()
        {
            Datalist.CurrentFilter.Id = "1";

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), Datalist.BaseGetModels());
        }

        [Test]
        public void FilterBySearchTermCalled()
        {
            Datalist.CurrentFilter.Id = null;

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), Datalist.BaseGetModels());
        }

        [Test]
        public void FilterBySearchTermFilteredCalled()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");
            Datalist.CurrentFilter.Id = null;

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), Datalist.BaseGetModels().Where(model => model.Id == "1"));
        }

        [Test]
        public void FilterBySearchTermNotCalled()
        {
            Datalist.CurrentFilter.Id = "1";

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterBySearchTerm", Times.Never(), Datalist.BaseGetModels());
        }

        [Test]
        public void FormDatalistDataCalled()
        {
            Datalist.GetData();
            DatalistMock.Protected().Verify("FormDatalistData", Times.Never(), Datalist.BaseGetModels());
        }

        #endregion

        #region Method: FilterById(IQueryable<T> models)

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NoId()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            datalist.BaseFilterById(new List<NoIdModel>().AsQueryable());
        }

        [Test]
        public void StringId()
        {
            Datalist.CurrentFilter.Id = "9";
            var expected = Datalist.BaseGetModels().Where(model => model.Id == Datalist.CurrentFilter.Id).ToList();
            var actual = Datalist.BaseFilterById(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void NumericId()
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
        [ExpectedException(typeof(DatalistException))]
        public void EnumId()
        {
            var datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.Id = IdEnum.Id.ToString();

            datalist.BaseFilterById(new List<EnumModel>().AsQueryable()).ToList();
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NonNumericId()
        {
            var datalist = new GenericDatalistStub<NonNumericIdModel>();
            datalist.CurrentFilter.Id = "9";

            datalist.BaseFilterById(new List<NonNumericIdModel>().AsQueryable()).ToList();
        }

        #endregion

        #region Method: FilterByAdditionalFilters(IQueryable<T> models)

        [Test]
        public void NullValues()
        {
            var expected = Datalist.BaseGetModels().ToList();
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", null);
            var actual = Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }


        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NoProperty()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("TestProperty", "Test");
            Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels());
        }

        [Test]
        public void Filter()
        {
            var numberFilter = 9;
            var stringFilter = "9";
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", stringFilter);
            Datalist.CurrentFilter.AdditionalFilters.Add("Number", numberFilter);
            var actual = Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model => model.Id == stringFilter && model.Number == numberFilter).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NotSupported()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("CreationDate", DateTime.Now);
            Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels());
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NotSupportedEnum()
        {
            var datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.AdditionalFilters.Add("IdEnum", DateTime.Now);

            datalist.BaseFilterByAdditionalFilters(new List<EnumModel>().AsQueryable()).ToList();
        }

        #endregion

        #region Method: FilterBySearchTerm(IQueryable<T> models)

        [Test]
        public void NullTerm()
        {
            Datalist.CurrentFilter.SearchTerm = null;
            var expected = Datalist.BaseGetModels().ToList();
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void FilterBySearchTerm_NoProperty()
        {
            Datalist.CurrentFilter.SearchTerm = "Test";
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels());
        }

        [Test]
        public void FilterBySearchTerm_WhiteSpaceTerm()
        {
            Datalist.CurrentFilter.SearchTerm = " ";
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm))).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void FilterBySearchTerm_ContainsSearch()
        {
            Datalist.CurrentFilter.SearchTerm = "1";
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)));

            CollectionAssert.AreEquivalent(expected.ToList(), actual);
        }

        [Test]
        public void FilterBySearchTerm_NoStringProperties()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("Number", String.Empty);
            Datalist.CurrentFilter.SearchTerm = "Test";

            var expected = Datalist.BaseGetModels().ToList();
            var actual = Datalist.BaseFilterBySearchTerm(expected.AsQueryable()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Sort(IQueryable<T> models)

        [Test]
        public void SortColumn()
        {
            Datalist.CurrentFilter.SortColumn = Datalist.BaseAttributedProperties.First().Name;
            var expected = Datalist.BaseGetModels().OrderBy(model => model.Number).ToList();
            var actual = Datalist.BaseSort(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void DefaultSortColumn()
        {
            Datalist.CurrentFilter.SortColumn = null;
            Datalist.BaseDefaultSortColumn = Datalist.BaseAttributedProperties.First().Name;
            var expected = Datalist.BaseGetModels().OrderBy(model => model.Number).ToList();
            var actual = Datalist.BaseSort(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NoSortColumn()
        {
            Datalist.CurrentFilter.SortColumn = "TestProperty";
            Datalist.BaseSort(Datalist.BaseGetModels());
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NoDefaultSortColumn()
        {
            Datalist.BaseDefaultSortColumn = "TestProperty";
            Datalist.CurrentFilter.SortColumn = null;
            Datalist.BaseSort(Datalist.BaseGetModels());
        }

        [Test]
        public void FirstColumnSort()
        {
            Datalist.BaseDefaultSortColumn = null;
            Datalist.CurrentFilter.SortColumn = null;
            var actual = Datalist.BaseSort(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().OrderBy(model => model.Number).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NoColumns()
        {
            Datalist.Columns.Clear();
            Datalist.BaseSort(Datalist.BaseGetModels());
        }

        #endregion

        #region Method: FormDatalistData(IQueryable<T> models)

        [Test]
        public void FilteredRecords()
        {
            var expected = Datalist.BaseGetModels().Count();
            var actual = Datalist.BaseFormDatalistData(Datalist.BaseGetModels()).FilteredRecords;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Columns()
        {
            var expected = Datalist.Columns;
            var actual = Datalist.BaseFormDatalistData(Datalist.BaseGetModels()).Columns;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Rows()
        {
            var expected = Datalist.CurrentFilter.RecordsPerPage;
            var actual = Datalist.BaseFormDatalistData(Datalist.BaseGetModels()).Rows.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RowModels()
        {
            Datalist.CurrentFilter.Page = 3;
            Datalist.CurrentFilter.RecordsPerPage = 3;
            Datalist.BaseFormDatalistData(Datalist.BaseGetModels());
            var expectedModels = Datalist.BaseGetModels().Skip(9).Take(3).ToList();
            var callCount = Math.Min(Datalist.CurrentFilter.RecordsPerPage, Datalist.BaseGetModels().Count());

            DatalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.Is<TestModel>(match => expectedModels.Contains(match)));
        }

        [Test]
        public void AddIdCalled()
        {
            Datalist.BaseFormDatalistData(Datalist.BaseGetModels());
            var callCount = Math.Min(Datalist.CurrentFilter.RecordsPerPage, Datalist.BaseGetModels().Count());
            DatalistMock.Protected().Verify("AddId", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void AddAutocompleteCalled()
        {
            Datalist.BaseFormDatalistData(Datalist.BaseGetModels());
            var callCount = Math.Min(Datalist.CurrentFilter.RecordsPerPage, Datalist.BaseGetModels().Count());
            DatalistMock.Protected().Verify("AddAutocomplete", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void AddColumnsCalled()
        {
            Datalist.BaseFormDatalistData(Datalist.BaseGetModels());
            var callCount = Math.Min(Datalist.CurrentFilter.RecordsPerPage, Datalist.BaseGetModels().Count());
            DatalistMock.Protected().Verify("AddColumns", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        [Test]
        public void AddAdditionalDataCalled()
        {
            Datalist.BaseFormDatalistData(Datalist.BaseGetModels());
            var callCount = Math.Min(Datalist.CurrentFilter.RecordsPerPage, Datalist.BaseGetModels().Count());
            DatalistMock.Protected().Verify("AddAdditionalData", Times.Exactly(callCount), ItExpr.IsAny<Dictionary<String, String>>(), ItExpr.IsAny<TestModel>());
        }

        #endregion

        #region Method: AddId(Dictionary<String, String> row, T model)

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void NoIdProperty()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            var row = new Dictionary<String, String>();
            var model = new NoIdModel();

            datalist.BaseAddId(row, model);
        }

        [Test]
        public void Key()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new TestModel());

            Assert.IsTrue(row.ContainsKey(AbstractDatalist.IdKey));
        }

        [Test]
        public void Value()
        {
            var row = new Dictionary<String, String>();
            var model = new TestModel() { Id = "Test" };

            Datalist.BaseAddId(row, model);

            Assert.IsTrue(row.ContainsValue(model.Id));
        }

        [Test]
        public void KeyCount()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }

        #endregion

        #region Method: AddAutocomplete(Dictionary<String, String> row, T model)

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void AddAutocomplete_NoColumns()
        {
            Datalist.Columns.Clear();
            Datalist.BaseAddAutocomplete(null, new TestModel());
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void AddAutocomplete_NoProperty()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseAddAutocomplete(new Dictionary<String, String>(), new TestModel());
        }

        [Test]
        public void AddAutocomplete_Key()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(AbstractDatalist.AcKey, row.First().Key);
        }

        [Test]
        public void AddAutocomplete_Value()
        {
            var model = new TestModel();
            var row = new Dictionary<String, String>();
            var firstProperty = model.GetType().GetProperty(Datalist.Columns.First().Key);
            Datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model).ToString(), row.First().Value);
        }

        [Test]
        public void AddAutocomplete_RelationValue()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("FirstRelationModel.Value", String.Empty);
            var model = new TestModel() { FirstRelationModel = new TestRelationModel() { Value = "Test" } };
            var firstProperty = typeof(TestRelationModel).GetProperty("Value");
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model.FirstRelationModel).ToString(), row.First().Value);
        }

        [Test]
        public void AddAutocomplete_KeyCount()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }

        #endregion

        #region Method: AddColumns(Dictionary<String, String> row, T model)

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void AddColumns_NoColumns()
        {
            Datalist.Columns.Clear();
            Datalist.BaseAddColumns(null, new TestModel());
        }

        [Test]
        [ExpectedException(typeof(DatalistException))]
        public void AddColumns_NoProperty()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseAddColumns(new Dictionary<String, String>(), new TestModel());
        }

        [Test]
        public void AddColumns_Keys()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddColumns(row, new TestModel());

            CollectionAssert.AreEqual(Datalist.Columns.Keys, row.Keys);
        }

        [Test]
        public void AddColumns_Values()
        {
            var expected = new List<String>();
            var row = new Dictionary<String, String>();
            var model = new TestModel() { FirstRelationModel = new TestRelationModel() { Value = "Test" } };
            foreach (KeyValuePair<String, String> column in Datalist.Columns)
                expected.Add(GetValue(model, column.Key));

            Datalist.BaseAddColumns(row, model);

            CollectionAssert.AreEqual(expected, row.Values);
        }

        [Test]
        public void AddColumns_KeyCount()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddColumns(row, new TestModel());

            Assert.AreEqual(Datalist.Columns.Count, row.Keys.Count);
        }

        #endregion

        #region Method: AddAdditionalData(Dictionary<String, String> row, T model)

        [Test]
        public void AddAdditionalData_KeyCount()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAdditionalData(row, new TestModel());

            Assert.AreEqual(0, row.Keys.Count);
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

            if (properties.Length == 1)
                value = property.GetValue(model);
            else
                value = GetValue(property.GetValue(model), String.Join(".", properties.Skip(1)));

            return value != null ? value.ToString() : String.Empty;
        }

        #endregion
    }
}
