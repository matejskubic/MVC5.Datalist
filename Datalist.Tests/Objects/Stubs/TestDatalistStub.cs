using Datalist.Tests.Objects.Data;
using Datalist.Tests.Objects.Models;
using System.Linq;

namespace Datalist.Tests.Objects.Stubs
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
