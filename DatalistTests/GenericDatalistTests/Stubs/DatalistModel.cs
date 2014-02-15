using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class DatalistModel
    {
        public const String DisplayValue = "Single display";

        [DatalistColumn(0)]
        public String Id { get; private set; }

        [DatalistColumn]
        [Display(Name = DisplayValue)]
        public Int32 Number { get; private set; }

        [DatalistColumn(-5)]
        public DateTime CreationDate { get; private set; }

        public Decimal Sum { get; private set; }

        [DatalistColumn(1, Relation = "Value")]
        public DatalistRelationModel FirstRelationModel { get; private set; }

        [DatalistColumn(1, Relation = "NoValue")]
        public DatalistRelationModel SecondRelationModel { get; private set; }

        public DatalistModel(Int32 index)
        {
            CreationDate = DateTime.Now.AddDays(index);
            Id = index.ToString();
            Sum = index + index;
            Number = (index % 2 == 0) ? index : -index;
            FirstRelationModel = (index % 2 == 0) ? new DatalistRelationModel()
            {
                Value = index.ToString(),
                NoValue = null
            } : null;
            SecondRelationModel = (index % 5 == 0) ? new DatalistRelationModel()
            {
                Value = index.ToString(),
                NoValue = null
            } : null;
        }
    }
}
