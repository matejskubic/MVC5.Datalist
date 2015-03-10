using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnsTests
    {
        private List<DatalistColumn> testColumns;
        private DatalistColumns columns;

        public DatalistColumnsTests()
        {
            columns = new DatalistColumns();
            testColumns = new List<DatalistColumn>
            {
                new DatalistColumn("Test1", String.Empty),
                new DatalistColumn("Test2", String.Empty),
            };
        }

        #region Property: Keys

        [Fact]
        public void Keys_EqualsToColumKeys()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            IEnumerable<String> expected = testColumns.Select(column => column.Key);
            IEnumerable<String> actual = testColumns.Select(column => column.Key);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: DatalistColumn()

        [Fact]
        public void DatalistColumn_EmptyColumn()
        {
            Assert.Empty(columns);
        }

        #endregion

        #region Method: Add(DatalistColumn column)

        [Fact]
        public void Add_OnNullColumnThrows()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(null));
        }

        [Fact]
        public void Add_OnSameColumnKeyThrows()
        {
            DatalistColumn column = new DatalistColumn("TestKey", String.Empty);
            columns.Add(column);

            Assert.Throws<DatalistException>(() => columns.Add(column));
        }

        [Fact]
        public void Add_AddsColumn()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            IEnumerable<DatalistColumn> expected = testColumns;
            IEnumerable<DatalistColumn> actual = columns;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Add(String key, String header, String cssClass = ")

        [Fact]
        public void Add_OnNullKeyThrows()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(null, String.Empty));
        }

        [Fact]
        public void Add_OnNullHeaderThrows()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(String.Empty, null));
        }

        [Fact]
        public void Add_OnNullCssClass()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(String.Empty, String.Empty, null));
        }

        [Fact]
        public void Add_OnSameKeyThrows()
        {
            Assert.Throws<DatalistException>(() =>
            {
                columns.Add("TestKey", String.Empty);
                columns.Add("TestKey", String.Empty);
            });
        }

        [Fact]
        public void Add_AddsColumnByValues()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column.Key, column.Header, column.CssClass);

            IEnumerator<DatalistColumn> expected = testColumns.GetEnumerator();
            IEnumerator<DatalistColumn> actual = columns.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.Equal(expected.Current.Key, actual.Current.Key);
                Assert.Equal(expected.Current.Header, actual.Current.Header);
                Assert.Equal(expected.Current.CssClass, actual.Current.CssClass);
            }
        }

        #endregion

        #region Method: Remove(DatalistColumn column)

        [Fact]
        public void Remove_RemovesColumn()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            DatalistColumn firstColumn = testColumns[0];
            testColumns.RemoveAt(0);

            Assert.True(columns.Remove(firstColumn));
            Assert.Equal(testColumns, columns);
        }

        [Fact]
        public void Remove_DoesNotRemoveColumn()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            Assert.False(columns.Remove(new DatalistColumn("Test1", String.Empty)));
            Assert.Equal(testColumns, columns);
        }

        [Fact]
        public void Remove_RemovesItSelf()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            foreach (DatalistColumn column in columns as IEnumerable)
                Assert.True(columns.Remove(column));

            Assert.Empty(columns);
        }

        #endregion

        #region Method: Remove(String key)

        [Fact]
        public void Remove_RemovesByKey()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            foreach (DatalistColumn column in columns)
                Assert.True(columns.Remove(column.Key));

            Assert.Empty(columns);
        }

        [Fact]
        public void Remove_DoesNotRemoveByKey()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            foreach (DatalistColumn column in columns)
                Assert.False(columns.Remove(column.Key + column.Key));

            Assert.Equal(testColumns, columns);
        }

        #endregion

        #region Method: Clear()

        [Fact]
        public void Clear_ClearsColumns()
        {
            columns.Add("Test1", String.Empty);
            columns.Add("Test2", String.Empty);
            columns.Clear();

            Assert.Empty(columns);
        }

        #endregion

        #region Method: GetEnumerator()

        [Fact]
        public void GetEnumerator_ReturnsColumnsCopy()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            foreach (DatalistColumn column in columns)
                columns.Remove(column);

            Assert.Empty(columns);
        }

        [Fact]
        public void GetEnumerator_ReturnsColumns()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            IEnumerable<DatalistColumn> actual = columns.ToArray();
            IEnumerable<DatalistColumn> expected = testColumns;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEnumerator_ReturnsSameColumns()
        {
            foreach (DatalistColumn column in testColumns)
                columns.Add(column);

            IEnumerable<DatalistColumn> expected = columns.ToArray();
            IEnumerable<DatalistColumn> actual = columns;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
