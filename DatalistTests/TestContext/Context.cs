using DatalistTests.Models;
using System.Data.Entity;

namespace DatalistTests.TestContext
{
    public class Context : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<TestRelationModel> TestRelationModels { get; set; }
    }
}
