namespace DatalistTests.Objects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestModels", "ParentId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestModels", "ParentId");
        }
    }
}
