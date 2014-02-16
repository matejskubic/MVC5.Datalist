using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class TestModel
    {
        public const String DisplayValue = "Single display";

        [DatalistColumn(0)]
        public String Id { get; set; }

        [DatalistColumn]
        [Display(Name = DisplayValue)]
        public Int32 Number { get; set; }

        [DatalistColumn(-5)]
        public DateTime CreationDate { get; set; }

        public Decimal Sum { get; set; }
        public String NullableString { get; set; }

        public String FirstRelationModelId { get; set; }
        public String SecondRelationModelId { get; set; }

        [DatalistColumn(1, Relation = "Value")]
        public virtual TestRelationModel FirstRelationModel { get; set; }

        [DatalistColumn(1, Relation = "NoValue")]
        public virtual TestRelationModel SecondRelationModel { get; set; }
    }
}
