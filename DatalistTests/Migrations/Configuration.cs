using DatalistTests.TestContext;
using DatalistTests.TestContext.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DatalistTests.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        private Context context;

        public Configuration()
        {
            context = new Context();
            AutomaticMigrationsEnabled = true;
            ContextKey = "Datalist.Tests.Data";
        }

        protected override void Seed(Context context)
        {
            for (Int32 i = 0; i < 100; i++)
            {
                String id = i.ToString();
                if (!context.TestModels.Any(model => model.Id == id))
                {
                    context.TestRelationModels.Add(new TestRelationModel() { Id = id, Value = id + "." + id, NoValue = id + "." + id + "." + id });
                    context.TestRelationModels.Add(new TestRelationModel() { Id = "-" + id, Value = "-" + id + "." + id, NoValue = "-" + id + "." + id + "." + id });
                    context.TestModels.Add(new TestModel(i));
                }
            }

            context.Save();
        }
    }
}
