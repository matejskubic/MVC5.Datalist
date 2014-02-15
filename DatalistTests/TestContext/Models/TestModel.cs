using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class TestModel
    {
        public const String DisplayValue = "Single display";

        [Key]
        [DatalistColumn(0)]
        public String Id { get; set; }

        [DatalistColumn]
        [Display(Name = DisplayValue)]
        public Int32 Number { get; set; }

        [DatalistColumn(-5)]
        public DateTime CreationDate { get; set; }

        public Decimal Sum { get; set; }

        public String FirstRelationModelId { get; set; }
        public String SecondRelationModelId { get; set; }

        [DatalistColumn(1, Relation = "Value")]
        public virtual TestRelationModel FirstRelationModel { get; set; }

        [DatalistColumn(1, Relation = "NoValue")]
        public virtual TestRelationModel SecondRelationModel { get; set; }

        public TestModel()
        {
        }
        public TestModel(Int32 index)
        {
            Id = index.ToString();
            Sum = index + index;
            Number = (index % 2 == 0) ? index : -index;
            CreationDate = DateTime.Now.AddDays(index);
            FirstRelationModelId = (index % 2 == 0) ? Id : null;
            SecondRelationModelId = (index % 5 == 0) ? "-" + Id : null;
        }
    }
}
