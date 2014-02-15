using DatalistTests.TestContext;
using DatalistTests.TestContext.Models;
using System.Linq;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class TestModelDatalistStub : GenericDatalistStub<TestModel>
    {
        private IQueryable<TestModel> models;

        public TestModelDatalistStub()
        {
            models = new Context().TestModels.OrderByDescending(model => model.Id);
        }

        protected override IQueryable<TestModel> GetModels()
        {
            return models;
        }
    }
}
