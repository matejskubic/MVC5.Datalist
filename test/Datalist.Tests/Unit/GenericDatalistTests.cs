using Datalist.Tests.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class GenericDatalistTests : IDisposable
    {
        private Dictionary<String, String> row;
        private TestDatalist<TestModel> datalist;

        public GenericDatalistTests()
        {
            HttpContext.Current = new HttpContext(
               new HttpRequest(null, "http://localhost:7013/", null),
               new HttpResponse(new StringWriter()));

            row = new Dictionary<String, String>();
            datalist = new TestDatalist<TestModel>();

            datalist.DefaultSortColumn = null;
            datalist.CurrentFilter.SearchTerm = null;

            for (Int32 i = 0; i < 20; i++)
                datalist.Models.Add(new TestModel
                {
                    Id = i + "I",
                    Count = i + 10,
                    Value = i + "V",
                    CreationDate = new DateTime(2014, 12, 10).AddDays(i)
                });
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region AttributedProperties

        [Fact]
        public void AttributedProperties_GetsOrderedProperties()
        {
            List<PropertyInfo> actual = datalist.AttributedProperties.ToList();
            List<PropertyInfo> expected = typeof(TestModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AbstractDatalist()

        [Fact]
        public void AbstractDatalist_AddsColumns()
        {
            DatalistColumns columns = new DatalistColumns();
            columns.Add(new DatalistColumn("Value", null));
            columns.Add(new DatalistColumn("CreationDate", "Date"));
            columns.Add(new DatalistColumn("Count", "Count's value"));

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
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => datalist.GetColumnKey(null));

            Assert.Equal("property", actual.ParamName);
        }

        [Fact]
        public void GetColumnKey_ReturnsPropertyName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Count");

            String actual = datalist.GetColumnKey(property);
            String expected = property.Name;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetColumnHeader(PropertyInfo property)

        [Fact]
        public void GetColumnHeader_NullProperty_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => datalist.GetColumnHeader(null));

            Assert.Equal("property", actual.ParamName);
        }

        [Fact]
        public void GetColumnHeader_NoDisplayName_ReturnsNull()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Value");

            String actual = datalist.GetColumnHeader(property);

            Assert.Null(actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsDisplayName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Count");

            String expected = property.GetCustomAttribute<DisplayAttribute>().Name;
            String actual = datalist.GetColumnHeader(property);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetColumnCssClass(PropertyInfo property)

        [Fact]
        public void GetColumnCssClass_ReturnsNull()
        {
            Assert.Null(datalist.GetColumnCssClass(null));
        }

        #endregion

        #region GetData()

        [Fact]
        public void GetData_FiltersById()
        {
            datalist.CurrentFilter.Id = "9I";
            datalist.CurrentFilter.SearchTerm = "Term";
            datalist.CurrentFilter.AdditionalFilters.Add("Value", "5V");

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 19).ToShortDateString(), actual.Rows[0]["CreationDate"]);
            Assert.Equal("9V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("9I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("9V", actual.Rows[0]["Value"]);
            Assert.Equal("19", actual.Rows[0]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(1, actual.FilteredRecords);
            Assert.Single(actual.Rows);
        }

        [Fact]
        public void GetData_FiltersByAdditionalFilters()
        {
            datalist.CurrentFilter.SearchTerm = "6V";
            datalist.CurrentFilter.AdditionalFilters.Add("Count", 16);

            datalist.GetData();

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 16).ToShortDateString(), actual.Rows[0]["CreationDate"]);
            Assert.Equal("6V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("6I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("6V", actual.Rows[0]["Value"]);
            Assert.Equal("16", actual.Rows[0]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(1, actual.FilteredRecords);
            Assert.Single(actual.Rows);
        }

        [Fact]
        public void GetData_FiltersBySearchTerm()
        {
            datalist.CurrentFilter.SearchTerm = "5V";

            datalist.GetData();

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 25).ToShortDateString(), actual.Rows[0]["CreationDate"]);
            Assert.Equal("15V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("15I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("15V", actual.Rows[0]["Value"]);
            Assert.Equal("25", actual.Rows[0]["Count"]);

            Assert.Equal(new DateTime(2014, 12, 15).ToShortDateString(), actual.Rows[1]["CreationDate"]);
            Assert.Equal("5V", actual.Rows[1][AbstractDatalist.AcKey]);
            Assert.Equal("5I", actual.Rows[1][AbstractDatalist.IdKey]);
            Assert.Equal("5V", actual.Rows[1]["Value"]);
            Assert.Equal("15", actual.Rows[1]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(2, actual.FilteredRecords);
            Assert.Equal(2, actual.Rows.Count);
        }

        [Fact]
        public void GetData_Sorts()
        {
            datalist.CurrentFilter.SortOrder = DatalistSortOrder.Asc;
            datalist.CurrentFilter.SortColumn = "Count";
            datalist.CurrentFilter.SearchTerm = "5V";

            datalist.GetData();

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 15).ToShortDateString(), actual.Rows[0]["CreationDate"]);
            Assert.Equal("5V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("5I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("5V", actual.Rows[0]["Value"]);
            Assert.Equal("15", actual.Rows[0]["Count"]);

            Assert.Equal(new DateTime(2014, 12, 25).ToShortDateString(), actual.Rows[1]["CreationDate"]);
            Assert.Equal("15V", actual.Rows[1][AbstractDatalist.AcKey]);
            Assert.Equal("15I", actual.Rows[1][AbstractDatalist.IdKey]);
            Assert.Equal("15V", actual.Rows[1]["Value"]);
            Assert.Equal("25", actual.Rows[1]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(2, actual.FilteredRecords);
            Assert.Equal(2, actual.Rows.Count);
        }

        #endregion

        #region FilterById(IQueryable<T> models)

        [Fact]
        public void FilterById_NoIdProperty_Throws()
        {
            TestDatalist<NoIdModel> datalist = new TestDatalist<NoIdModel>();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.FilterById(null));

            String expected = $"'{typeof(NoIdModel).Name}' type does not have property named 'Id'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_String()
        {
            datalist.CurrentFilter.Id = "9I";

            IQueryable<TestModel> expected = datalist.GetModels().Where(model => model.Id == datalist.CurrentFilter.Id);
            IQueryable<TestModel> actual = datalist.FilterById(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_Number()
        {
            TestDatalist<NumericModel> datalist = new TestDatalist<NumericModel>();
            for (Int32 i = 0; i < 20; i++)
                datalist.Models.Add(new NumericModel { Id = i });

            datalist.CurrentFilter.Id = "9.0";

            IQueryable<NumericModel> expected = datalist.GetModels().Where(model => model.Id == 9);
            IQueryable<NumericModel> actual = datalist.FilterById(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_NotSupportedIdType_Throws()
        {
            DatalistException exception = Assert.Throws<DatalistException>(() => new TestDatalist<GuidModel>().FilterById(null));

            String expected = $"'{typeof(GuidModel).Name}.Id' can not be filtered by using '' value, because it's not a string nor a number.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FilterByAdditionalFilters(IQueryable<T> models)

        [Fact]
        public void FilterByAdditionalFilters_SkipsNullValues()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", null);

            IQueryable<TestModel> actual = datalist.FilterByAdditionalFilters(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterByAdditionalFilters_Filters()
        {
            datalist.CurrentFilter.AdditionalFilters.Add("Id", "9I");
            datalist.CurrentFilter.AdditionalFilters.Add("Count", 9);
            datalist.CurrentFilter.AdditionalFilters.Add("CreationDate", new DateTime(2014, 12, 15));

            IQueryable<TestModel> actual = datalist.FilterByAdditionalFilters(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels().Where(model =>
                model.Id == "9I" && model.Count == 9 && model.CreationDate == new DateTime(2014, 12, 15));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FilterBySearchTerm(IQueryable<T> models)

        [Fact]
        public void FilterBySearchTerm_SkipsNullTerm()
        {
            datalist.CurrentFilter.SearchTerm = null;

            IQueryable<TestModel> actual = datalist.FilterBySearchTerm(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_NoProperty_Throws()
        {
            datalist.CurrentFilter.SearchTerm = "Test";
            datalist.Columns.Add(new DatalistColumn("Test", "Test"));

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.FilterBySearchTerm(datalist.GetModels()));

            String expected = $"'{typeof(TestModel).Name}' type does not have property named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_UsesContainsSearch()
        {
            datalist.CurrentFilter.SearchTerm = "1";

            IQueryable<TestModel> expected = datalist.GetModels().Where(model => model.Id.Contains("1"));
            IQueryable<TestModel> actual = datalist.FilterBySearchTerm(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearchTerm_DoesNotFilterNonStringProperties()
        {
            datalist.Columns.Clear();
            datalist.CurrentFilter.SearchTerm = "1";
            datalist.Columns.Add(new DatalistColumn("Count", ""));

            IQueryable<TestModel> actual = datalist.FilterBySearchTerm(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Sort(IQueryable<T> models)

        [Fact]
        public void Sort_ByColumn()
        {
            datalist.CurrentFilter.SortColumn = "Count";

            IQueryable<TestModel> expected = datalist.GetModels().OrderBy(model => model.Count);
            IQueryable<TestModel> actual = datalist.Sort(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_ByDefaultSortColumn()
        {
            datalist.DefaultSortColumn = "Count";
            datalist.CurrentFilter.SortColumn = null;

            IQueryable<TestModel> expected = datalist.GetModels().OrderBy(model => model.Count);
            IQueryable<TestModel> actual = datalist.Sort(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_NoColumn_Throws()
        {
            datalist.CurrentFilter.SortColumn = "Test";

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.Sort(datalist.GetModels()));

            String expected = "Datalist does not contain sort column named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_NoDefaultProperty_Throws()
        {
            datalist.DefaultSortColumn = "Test";
            datalist.CurrentFilter.SortColumn = null;

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.Sort(datalist.GetModels()));

            String expected = "Datalist does not contain sort column named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_ByFirstColumn()
        {
            datalist.DefaultSortColumn = null;
            datalist.CurrentFilter.SortColumn = null;

            IQueryable<TestModel> expected = datalist.GetModels().OrderBy(model => model.Value);
            IQueryable<TestModel> actual = datalist.Sort(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_NoColumns_Throws()
        {
            datalist.Columns.Clear();
            datalist.DefaultSortColumn = null;
            datalist.CurrentFilter.SortColumn = null;

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.Sort(datalist.GetModels()));

            String expected = "Datalist should have at least one column.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDatalistData(IQueryable<T> models)

        [Fact]
        public void FormDatalistData_FilteredRecords()
        {
            Int32 actual = datalist.FormDatalistData(datalist.GetModels()).FilteredRecords;
            Int32 expected = datalist.GetModels().Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormDatalistData_Columns()
        {
            DatalistColumns actual = datalist.FormDatalistData(datalist.GetModels()).Columns;
            DatalistColumns expected = datalist.Columns;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormDatalistData_Rows()
        {
            datalist.CurrentFilter.Page = 2;
            datalist.CurrentFilter.RecordsPerPage = 3;

            IEnumerator<Dictionary<String, String>> actual = datalist.FormDatalistData(datalist.GetModels()).Rows.GetEnumerator();
            IEnumerator<Dictionary<String, String>> expected = new List<Dictionary<String, String>>
            {
                new Dictionary<String, String>
                {
                    [AbstractDatalist.IdKey] = "6I",
                    [AbstractDatalist.AcKey] = "6V",
                    ["Value"] = "6V",
                    ["CreationDate"] = new DateTime(2014, 12, 16).ToShortDateString(),
                    ["Count"] = "16"
                },
                new Dictionary<String, String>
                {
                    [AbstractDatalist.IdKey] = "7I",
                    [AbstractDatalist.AcKey] = "7V",
                    ["Value"] = "7V",
                    ["CreationDate"] = new DateTime(2014, 12, 17).ToShortDateString(),
                    ["Count"] = "17"
                },
                new Dictionary<String, String>
                {
                    [AbstractDatalist.IdKey] = "8I",
                    [AbstractDatalist.AcKey] = "8V",
                    ["Value"] = "8V",
                    ["CreationDate"] = new DateTime(2014, 12, 18).ToShortDateString(),
                    ["Count"] = "18"
                }
            }.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Values, actual.Current.Values);
                Assert.Equal(expected.Current.Keys, actual.Current.Keys);
            }
        }

        #endregion

        #region AddId(Dictionary<String, String> row, T model)

        [Fact]
        public void AddId_NoProperty_Throws()
        {
            TestDatalist<NoIdModel> datalist = new TestDatalist<NoIdModel>();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.AddId(row, new NoIdModel()));

            String expected = $"'{typeof(NoIdModel).Name}' type does not have property named 'Id'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddId_Values()
        {
            datalist.AddId(row, new TestModel { Id = "Test" });

            KeyValuePair<String, String> actual = row.Single();

            Assert.Equal(AbstractDatalist.IdKey, actual.Key);
            Assert.Equal("Test", actual.Value);
        }

        #endregion

        #region AddAutocomplete(Dictionary<String, String> row, T model)

        [Fact]
        public void AddAutocomplete_NoColumns_Throws()
        {
            datalist.Columns.Clear();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.AddAutocomplete(row, new TestModel()));

            String expected = "Datalist should have at least one column.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAutocomplete_NoProperty_Throws()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("Test", ""));

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.AddAutocomplete(row, new TestModel()));

            String expected = $"'{typeof(TestModel).Name}' type does not have property named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAutocomplete_Values()
        {
            datalist.AddAutocomplete(row, new TestModel { Value = "Test" });

            KeyValuePair<String, String> actual = row.Single();

            Assert.Equal(AbstractDatalist.AcKey, actual.Key);
            Assert.Equal("Test", actual.Value);
        }

        #endregion

        #region AddColumns(Dictionary<String, String> row, T model)

        [Fact]
        public void AddColumns_NoColumns_Throws()
        {
            datalist.Columns.Clear();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.AddColumns(null, new TestModel()));

            String expected = "Datalist should have at least one column.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddColumns_NoProperty_Throws()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("Test", ""));

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.AddColumns(row, new TestModel()));

            String expected = $"'{typeof(TestModel).Name}' type does not have property named 'Test'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddColumns_Values()
        {
            datalist.AddColumns(row, new TestModel { Value = "Test", CreationDate = DateTime.Now.Date, Count = 4 });

            Assert.Equal(new[] { "Test", DateTime.Now.Date.ToShortDateString(), "4" }, row.Values);
            Assert.Equal(datalist.Columns.Keys, row.Keys);
        }

        #endregion

        #region AddAdditionalData(Dictionary<String, String> row, T model)

        [Fact]
        public void AddAdditionalData_DoesNothing()
        {
            datalist.AddAdditionalData(row, new TestModel());

            Assert.Empty(row.Keys);
        }

        #endregion
    }
}
