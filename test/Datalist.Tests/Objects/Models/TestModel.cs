using System;
using System.ComponentModel.DataAnnotations;

namespace Datalist.Tests.Objects
{
    public class TestModel
    {
        public String Id { get; set; }

        [DatalistColumn(8)]
        [Display(Name = "Count's value")]
        public Int32 Count { get; set; }

        [DatalistColumn]
        public String Value { get; set; }

        [Datalist(typeof(TestDatalist<TestModel>))]
        public String ParentId { get; set; }

        [Display(Name = "Date")]
        [DatalistColumn(-3, Format = "{0:d}")]
        public DateTime CreationDate { get; set; }

        public String NullableString { get; set; }

        public String RelationId { get; set; }
        public virtual TestRelationModel Relation { get; set; }
    }
}
