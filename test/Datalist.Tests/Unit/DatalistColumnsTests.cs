using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnsTests
    {
        private List<DatalistColumn> allColumns;
        private DatalistColumns columns;

        public DatalistColumnsTests()
        {
            columns = new DatalistColumns();
            allColumns = new List<DatalistColumn>();
            allColumns.Add(new DatalistColumn("Test1", "Header1"));
            allColumns.Add(new DatalistColumn("Test2", "Header2"));

            foreach (DatalistColumn column in allColumns)
                columns.Add(column);
        }

        #region Keys

        [Fact]
        public void Keys_ReturnsColumnKeys()
        {
            IEnumerable<String> expected = new[] { "Test1", "Test2" };
            IEnumerable<String> actual = columns.Keys;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region DatalistColumns()

        [Fact]
        public void DatalistColumnsEmpty()
        {
            Assert.Empty(new DatalistColumns());
        }

        #endregion

        #region Add(DatalistColumn column)

        [Fact]
        public void Add_NullColumn_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => columns.Add(null));

            Assert.Equal("column", actual.ParamName);
        }

        [Fact]
        public void Add_SameColumnKey_Throws()
        {
            DatalistException exception = Assert.Throws<DatalistException>(() => columns.Add(columns.First()));

            String expected = $"Can not add datalist column with the same key '{columns.First().Key}'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_Column()
        {
            DatalistColumn column = new DatalistColumn("Test3", "3");
            allColumns.Add(column);

            columns.Add(column);

            IEnumerable<DatalistColumn> expected = allColumns;
            IEnumerable<DatalistColumn> actual = columns;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Add(String key, String header, String cssClass = null)

        [Fact]
        public void Add_NullKey_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => columns.Add(null, ""));

            Assert.Equal("key", actual.ParamName);
        }

        [Fact]
        public void Add_SameKey_Throws()
        {
            DatalistException exception = Assert.Throws<DatalistException>(() => columns.Add(columns.First().Key, "1"));

            String expected = $"Can not add datalist column with the same key '{columns.First().Key}'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_ColumnFromValues()
        {
            columns = new DatalistColumns();
            foreach (DatalistColumn column in allColumns)
                columns.Add(column.Key, column.Header, column.CssClass);

            IEnumerator<DatalistColumn> expected = allColumns.GetEnumerator();
            IEnumerator<DatalistColumn> actual = columns.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Key, actual.Current.Key);
                Assert.Equal(expected.Current.Header, actual.Current.Header);
                Assert.Equal(expected.Current.CssClass, actual.Current.CssClass);
            }
        }

        #endregion

        #region Remove(DatalistColumn column)

        [Fact]
        public void Remove_NoColumn()
        {
            Assert.False(columns.Remove(new DatalistColumn("Test1", "Header1")));
            Assert.Equal(allColumns, columns);
        }

        [Fact]
        public void Remove_Column()
        {
            Assert.True(columns.Remove(columns.First()));
            Assert.Equal(allColumns.Skip(1), columns);
        }

        [Fact]
        public void Remove_ItSelf()
        {
            foreach (DatalistColumn column in columns)
                Assert.True(columns.Remove(column));

            Assert.Empty(columns);
        }

        #endregion

        #region Remove(String key)

        [Fact]
        public void Remove_ByKey()
        {
            foreach (DatalistColumn column in columns)
                Assert.True(columns.Remove(column.Key));

            Assert.Empty(columns);
        }

        [Fact]
        public void Remove_NoKey()
        {
            Assert.False(columns.Remove("Test3"));
            Assert.Equal(allColumns, columns);
        }

        #endregion

        #region Clear()

        [Fact]
        public void Clear_Columns()
        {
            columns.Clear();

            Assert.Empty(columns);
        }

        #endregion

        #region GetEnumerator()

        [Fact]
        public void GetEnumerator_ReturnsColumns()
        {
            IEnumerable<DatalistColumn> actual = columns.ToArray();
            IEnumerable<DatalistColumn> expected = allColumns;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
