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
        private DatalistColumns columns;

        [SetUp]
        public void SetUp()
        {
            columns = new DatalistColumns();
        }


        #region Property: Keys

        [Test]
        public void Keys_EqualsToColumKeys()
        {
            var addedColumns = new List<DatalistColumn>()
            {
                new DatalistColumn("Test1", String.Empty),
                new DatalistColumn("Test2", String.Empty),
            };
            foreach (var column in addedColumns)
                columns.Add(column);

            CollectionAssert.AreEqual(addedColumns.Select(column => column.Key), columns.Keys);
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
        public void Add_OnNullColumnKeyThrows()
        {
            Assert.Throws<DatalistException>(() => columns.Add(new DatalistColumn()));
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
            var addedColumns = new List<DatalistColumn>()
            {
                new DatalistColumn("Test1", String.Empty),
                new DatalistColumn("Test2", String.Empty),
            };
            foreach (var column in addedColumns)
                columns.Add(column);

            CollectionAssert.AreEqual(addedColumns, columns);
        }

        #endregion

        #region Method: Remove(DatalistColumn column)

        [Test]
        public void Remove_RemovesColumn()
        {
            var addedColumns = new List<DatalistColumn>()
            {
                new DatalistColumn("Test1", String.Empty),
                new DatalistColumn("Test2", String.Empty),
            };
            foreach (var column in addedColumns)
                columns.Add(column);

            Assert.IsTrue(columns.Remove(addedColumns[0]));

            addedColumns.RemoveAt(0);
            CollectionAssert.AreEqual(addedColumns, columns);
        }

        [Test]
        public void Remove_RemovesItSelf()
        {
            columns.Add(new DatalistColumn("Test1", String.Empty));
            columns.Add(new DatalistColumn("Test2", String.Empty));

            foreach (var column in columns as IEnumerable)
                columns.Remove(column as DatalistColumn);

            CollectionAssert.IsEmpty(columns);
        }

        #endregion

        #region Method: Clear()

        [Test]
        public void Clear_ClearsColumns()
        {
            columns.Add(new DatalistColumn("Test1", String.Empty));
            columns.Add(new DatalistColumn("Test2", String.Empty));
            columns.Clear();

            CollectionAssert.IsEmpty(columns);
        }

        #endregion
    }
}
