using System.Data.Entity.Migrations;

namespace Datalist.Tests.Objects.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Datalist.Tests.Data";
            MigrationsDirectory = "Objects\\Data\\Migrations";
        }
    }
}
