using DatalistTests.TestContext;
using DatalistTests.Models;
using System.Linq;

namespace DatalistTests.Stubs
{
    public class TestDatalistStub : GenericDatalistStub<TestModel>
    {
        private IQueryable<TestModel> models;

        public TestDatalistStub()
        {
            models = new Context().TestModels.OrderByDescending(model => model.Id);
        }

        protected override IQueryable<TestModel> GetModels()
        {
            return models;
        }
    }
}
