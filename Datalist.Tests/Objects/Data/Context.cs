using Datalist.Tests.Objects.Models;
using System.Data.Entity;

namespace Datalist.Tests.Objects.Data
{
    public class Context : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<TestRelationModel> TestRelationModels { get; set; }
    }
}
