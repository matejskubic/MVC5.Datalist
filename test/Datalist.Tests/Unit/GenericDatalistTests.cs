using Datalist.Tests.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Xunit;
using Xunit.Extensions;

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

            for (Int32 i = 0; i < 20; i++)
                datalist.Models.Add(new TestModel
                {
                    Id = i + "I",
                    Count = i + 10,
                    Value = i + "V",
                    Date = new DateTime(2014, 12, 10).AddDays(i)
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
            columns.Add(new DatalistColumn("Date", "Date"));
            columns.Add(new DatalistColumn("Count", "Value"));

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
        public void GetColumnHeader_NullProperty_ReturnsNull()
        {
            Assert.Null(datalist.GetColumnHeader(null));
        }

        [Fact]
        public void GetColumnHeader_NoDisplayName_ReturnsNull()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Value");

            Assert.Null(datalist.GetColumnHeader(property));
        }

        [Fact]
        public void GetColumnHeader_ReturnsDisplayName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Date");

            String expected = property.GetCustomAttribute<DisplayAttribute>().Name;
            String actual = datalist.GetColumnHeader(property);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsDisplayShortName()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Count");

            String expected = property.GetCustomAttribute<DisplayAttribute>().ShortName;
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
            datalist.Filter.Id = "9I";
            datalist.Filter.Search = "Term";
            datalist.Filter.AdditionalFilters.Add("Value", "5V");

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 19).ToShortDateString(), actual.Rows[0]["Date"]);
            Assert.Equal("9V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("9I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("9V", actual.Rows[0]["Value"]);
            Assert.Equal("19", actual.Rows[0]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(1, actual.FilteredRows);
            Assert.Single(actual.Rows);
        }

        [Fact]
        public void GetData_FiltersByAdditionalFilters()
        {
            datalist.Filter.Search = "6V";
            datalist.Filter.AdditionalFilters.Add("Count", 16);

            datalist.GetData();

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 16).ToShortDateString(), actual.Rows[0]["Date"]);
            Assert.Equal("6V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("6I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("6V", actual.Rows[0]["Value"]);
            Assert.Equal("16", actual.Rows[0]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(1, actual.FilteredRows);
            Assert.Single(actual.Rows);
        }

        [Fact]
        public void GetData_FiltersBySearch()
        {
            datalist.Filter.Search = "5V";

            datalist.GetData();

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 25).ToShortDateString(), actual.Rows[0]["Date"]);
            Assert.Equal("15V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("15I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("15V", actual.Rows[0]["Value"]);
            Assert.Equal("25", actual.Rows[0]["Count"]);

            Assert.Equal(new DateTime(2014, 12, 15).ToShortDateString(), actual.Rows[1]["Date"]);
            Assert.Equal("5V", actual.Rows[1][AbstractDatalist.AcKey]);
            Assert.Equal("5I", actual.Rows[1][AbstractDatalist.IdKey]);
            Assert.Equal("5V", actual.Rows[1]["Value"]);
            Assert.Equal("15", actual.Rows[1]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(2, actual.FilteredRows);
            Assert.Equal(2, actual.Rows.Count);
        }

        [Fact]
        public void GetData_Sorts()
        {
            datalist.Filter.SortOrder = DatalistSortOrder.Asc;
            datalist.Filter.SortColumn = "Count";
            datalist.Filter.Search = "5V";

            datalist.GetData();

            DatalistData actual = datalist.GetData();

            Assert.Equal(new DateTime(2014, 12, 15).ToShortDateString(), actual.Rows[0]["Date"]);
            Assert.Equal("5V", actual.Rows[0][AbstractDatalist.AcKey]);
            Assert.Equal("5I", actual.Rows[0][AbstractDatalist.IdKey]);
            Assert.Equal("5V", actual.Rows[0]["Value"]);
            Assert.Equal("15", actual.Rows[0]["Count"]);

            Assert.Equal(new DateTime(2014, 12, 25).ToShortDateString(), actual.Rows[1]["Date"]);
            Assert.Equal("15V", actual.Rows[1][AbstractDatalist.AcKey]);
            Assert.Equal("15I", actual.Rows[1][AbstractDatalist.IdKey]);
            Assert.Equal("15V", actual.Rows[1]["Value"]);
            Assert.Equal("25", actual.Rows[1]["Count"]);

            Assert.Equal(datalist.Columns, actual.Columns);
            Assert.Equal(2, actual.FilteredRows);
            Assert.Equal(2, actual.Rows.Count);
        }

        #endregion

        #region FilterById(IQueryable<T> models)

        [Fact]
        public void FilterById_NoIdProperty_Throws()
        {
            TestDatalist<NoIdModel> datalist = new TestDatalist<NoIdModel>();

            DatalistException exception = Assert.Throws<DatalistException>(() => datalist.FilterById(null));

            String expected = $"'{typeof(NoIdModel).Name}' type does not have property named 'Id', required for automatic id filtering.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_String()
        {
            datalist.Filter.Id = "9I";

            IQueryable<TestModel> expected = datalist.GetModels().Where(model => model.Id == datalist.Filter.Id);
            IQueryable<TestModel> actual = datalist.FilterById(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_Number()
        {
            TestDatalist<NumericModel> datalist = new TestDatalist<NumericModel>();
            for (Int32 i = 0; i < 20; i++)
                datalist.Models.Add(new NumericModel { Id = i });

            datalist.Filter.Id = "9.0";

            IQueryable<NumericModel> expected = datalist.GetModels().Where(model => model.Id == 9);
            IQueryable<NumericModel> actual = datalist.FilterById(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterById_NotSupportedIdType_Throws()
        {
            DatalistException exception = Assert.Throws<DatalistException>(() => new TestDatalist<GuidModel>().FilterById(null));

            String expected = $"'{typeof(GuidModel).Name}.Id' property type has to be a string or a number.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FilterByAdditionalFilters(IQueryable<T> models)

        [Fact]
        public void FilterByAdditionalFilters_SkipsNullValues()
        {
            datalist.Filter.AdditionalFilters.Add("Id", null);

            IQueryable<TestModel> actual = datalist.FilterByAdditionalFilters(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterByAdditionalFilters_Filters()
        {
            datalist.Filter.AdditionalFilters.Add("Id", "9I");
            datalist.Filter.AdditionalFilters.Add("Count", 9);
            datalist.Filter.AdditionalFilters.Add("Date", new DateTime(2014, 12, 15));

            IQueryable<TestModel> actual = datalist.FilterByAdditionalFilters(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels().Where(model =>
                model.Id == "9I" && model.Count == 9 && model.Date == new DateTime(2014, 12, 15));

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FilterBySearch(IQueryable<T> models)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FilterBySearch_SkipsEmptyTerm(String term)
        {
            datalist.Filter.Search = term;

            IQueryable<TestModel> actual = datalist.FilterBySearch(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearch_DoesNotFilterNotExistingProperties()
        {
            datalist.Columns.Clear();
            datalist.Filter.Search = "1";
            datalist.Columns.Add(new DatalistColumn("Test", "Test"));

            IQueryable<TestModel> actual = datalist.FilterBySearch(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearch_UsesContainsSearch()
        {
            datalist.Filter.Search = "1";

            IQueryable<TestModel> expected = datalist.GetModels().Where(model => model.Id.Contains("1"));
            IQueryable<TestModel> actual = datalist.FilterBySearch(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBySearch_DoesNotFilterNonStringProperties()
        {
            datalist.Columns.Clear();
            datalist.Filter.Search = "1";
            datalist.Columns.Add(new DatalistColumn("Count", null));

            IQueryable<TestModel> actual = datalist.FilterBySearch(datalist.GetModels());
            IQueryable<TestModel> expected = datalist.GetModels();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Sort(IQueryable<T> models)

        [Fact]
        public void Sort_ByColumn()
        {
            datalist.Filter.SortColumn = "Count";

            IQueryable<TestModel> expected = datalist.GetModels().OrderBy(model => model.Count);
            IQueryable<TestModel> actual = datalist.Sort(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sort_ByFirstColumn()
        {
            datalist.Filter.SortColumn = null;

            IQueryable<TestModel> expected = datalist.GetModels().OrderBy(model => model.Value);
            IQueryable<TestModel> actual = datalist.Sort(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Sort_NoSortColumns(String column)
        {
            datalist.Columns.Clear();
            datalist.Filter.SortColumn = column;

            IQueryable<TestModel> expected = datalist.GetModels();
            IQueryable<TestModel> actual = datalist.Sort(datalist.GetModels());

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormDatalistData(IQueryable<T> models)

        [Fact]
        public void FormDatalistData_FilteredRows()
        {
            Int32 actual = datalist.FormDatalistData(datalist.GetModels()).FilteredRows;
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
            datalist.Filter.Page = 2;
            datalist.Filter.Rows = 3;

            IEnumerator<Dictionary<String, String>> actual = datalist.FormDatalistData(datalist.GetModels()).Rows.GetEnumerator();
            IEnumerator<Dictionary<String, String>> expected = new List<Dictionary<String, String>>
            {
                new Dictionary<String, String>
                {
                    [AbstractDatalist.IdKey] = "6I",
                    [AbstractDatalist.AcKey] = "6V",
                    ["Value"] = "6V",
                    ["Date"] = new DateTime(2014, 12, 16).ToShortDateString(),
                    ["Count"] = "16"
                },
                new Dictionary<String, String>
                {
                    [AbstractDatalist.IdKey] = "7I",
                    [AbstractDatalist.AcKey] = "7V",
                    ["Value"] = "7V",
                    ["Date"] = new DateTime(2014, 12, 17).ToShortDateString(),
                    ["Count"] = "17"
                },
                new Dictionary<String, String>
                {
                    [AbstractDatalist.IdKey] = "8I",
                    [AbstractDatalist.AcKey] = "8V",
                    ["Value"] = "8V",
                    ["Date"] = new DateTime(2014, 12, 18).ToShortDateString(),
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
        public void AddId_EmptyValues()
        {
            TestDatalist<NoIdModel> datalist = new TestDatalist<NoIdModel>();

            datalist.AddId(row, new NoIdModel());

            KeyValuePair<String, String> actual = row.Single();

            Assert.Equal(AbstractDatalist.IdKey, actual.Key);
            Assert.Null(actual.Value);
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
        public void AddAutocomplete_EmptyValues()
        {
            datalist.Columns.Clear();

            datalist.AddAutocomplete(row, new TestModel());

            KeyValuePair<String, String> actual = row.Single();

            Assert.Equal(AbstractDatalist.AcKey, actual.Key);
            Assert.Null(actual.Value);
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
        public void AddColumns_EmptyValues()
        {
            datalist.Columns.Clear();
            datalist.Columns.Add(new DatalistColumn("Test", null));

            datalist.AddColumns(row, new TestModel { Value = "Test", Date = DateTime.Now.Date, Count = 4 });

            Assert.Equal(new String[] { null }, row.Values);
            Assert.Equal(datalist.Columns.Keys, row.Keys);
        }

        [Fact]
        public void AddColumns_Values()
        {
            datalist.AddColumns(row, new TestModel { Value = "Test", Date = DateTime.Now.Date, Count = 4 });

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
