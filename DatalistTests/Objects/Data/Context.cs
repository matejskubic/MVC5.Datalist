using DatalistTests.Objects.Models;
using System.Data.Entity;

namespace DatalistTests.Objects.Data
{
    public class Context : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<TestRelationModel> TestRelationModels { get; set; }
    }
}
