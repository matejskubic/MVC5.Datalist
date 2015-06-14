using Datalist.Tests.Objects.Data;
using System.Linq;

namespace Datalist.Tests.Objects
{
    public class TestDatalistProxy : GenericDatalistProxy<TestModel>
    {
        private IQueryable<TestModel> models;

        public TestDatalistProxy()
        {
            models = new Context().TestModels.OrderByDescending(model => model.Id);
        }

        protected override IQueryable<TestModel> GetModels()
        {
            return models;
        }
    }
}
