using Datalist;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistColumnsTests
    {
        private List<DatalistColumn> testColumns;
        private DatalistColumns columns;

        [SetUp]
        public void SetUp()
        {
            columns = new DatalistColumns();
            testColumns = new List<DatalistColumn>()
            {
                new DatalistColumn("Test1", String.Empty),
                new DatalistColumn("Test2", String.Empty),
            };
        }

        #region Property: Keys

        [Test]
        public void Keys_EqualsToColumKeys()
        {
            foreach (var column in testColumns)
                columns.Add(column);

            CollectionAssert.AreEqual(testColumns.Select(column => column.Key), columns.Keys);
        }

        #endregion

        #region Constructor: DatalistColumn()

        [Test]
        public void DatalistColumn_EmptyColumn()
        {
            CollectionAssert.IsEmpty(columns);
        }

        #endregion

        #region Method: Add(DatalistColumn column)

        [Test]
        public void Add_OnNullColumnThrows()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(null));
        }

        [Test]
        public void Add_OnSameColumnKeyThrows()
        {
            var column = new DatalistColumn("TestKey", String.Empty);
            columns.Add(column);

            Assert.Throws<DatalistException>(() => columns.Add(column));
        }

        [Test]
        public void Add_AddsColumn()
        {
            foreach (var column in testColumns)
                columns.Add(column);

            CollectionAssert.AreEqual(testColumns, columns);
        }

        #endregion

        #region Method: Add(String key, String header, String cssClass = ")

        [Test]
        public void Add_OnNullKeyThrows()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(null, String.Empty));
        }

        [Test]
        public void Add_OnNullHeaderThrows()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(String.Empty, null));
        }

        [Test]
        public void Add_OnNullCssClass()
        {
            Assert.Throws<ArgumentNullException>(() => columns.Add(String.Empty, String.Empty, null));
        }

        [Test]
        public void Add_OnSameKeyThrows()
        {
            Assert.Throws<DatalistException>(() =>
            {
                columns.Add("TestKey", String.Empty);
                columns.Add("TestKey", String.Empty);
            });
        }

        [Test]
        public void Add_AddsColumnByValues()
        {
            foreach (var column in testColumns)
                columns.Add(column.Key, column.Header, column.CssClass);


            var expected = testColumns.GetEnumerator();
            var actual = columns.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Key, actual.Current.Key);
                Assert.AreEqual(expected.Current.Header, actual.Current.Header);
                Assert.AreEqual(expected.Current.CssClass, actual.Current.CssClass);
            }
        }

        #endregion

        #region Method: Remove(DatalistColumn column)

        [Test]
        public void Remove_RemovesColumn()
        {
            foreach (var column in testColumns)
                columns.Add(column);

            Assert.IsTrue(columns.Remove(testColumns[0]));

            testColumns.RemoveAt(0);
            CollectionAssert.AreEqual(testColumns, columns);
        }

        [Test]
        public void Remove_DoesNotRemoveColumn()
        {
            foreach (var column in testColumns)
                columns.Add(column);

            Assert.IsFalse(columns.Remove(new DatalistColumn("Test1", String.Empty)));
            CollectionAssert.AreEqual(testColumns, columns);
        }

        [Test]
        public void Remove_RemovesItSelf()
        {
            foreach (var column in testColumns)
                columns.Add(column);
            foreach (var column in columns as IEnumerable)
                Assert.IsTrue(columns.Remove(column as DatalistColumn));

            CollectionAssert.IsEmpty(columns);
        }

        #endregion

        #region Method: Remove(String key)

        [Test]
        public void Remove_RemovesByKey()
        {
            foreach (var column in testColumns)
                columns.Add(column);

            foreach (var column in columns)
                Assert.IsTrue(columns.Remove(column.Key));

            CollectionAssert.IsEmpty(columns);
        }

        [Test]
        public void Remove_DoesNotRemoveByKey()
        {
            foreach (var column in testColumns)
                columns.Add(column);

            foreach (var column in columns)
                Assert.IsFalse(columns.Remove(column.Key + column.Key));

            CollectionAssert.AreEqual(testColumns, columns);
        }

        #endregion

        #region Method: Clear()

        [Test]
        public void Clear_ClearsColumns()
        {
            columns.Add("Test1", String.Empty);
            columns.Add("Test2", String.Empty);
            columns.Clear();

            CollectionAssert.IsEmpty(columns);
        }

        #endregion
    }
}
