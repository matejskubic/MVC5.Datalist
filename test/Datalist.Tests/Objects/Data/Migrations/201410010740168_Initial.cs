namespace Datalist.Tests.Objects.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Number = c.Int(nullable: false),
                        ParentId = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NullableString = c.String(),
                        RelationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestRelationModels", t => t.RelationId)
                .Index(t => t.RelationId);
            
            CreateTable(
                "dbo.TestRelationModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                        NoValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestModels", "RelationId", "dbo.TestRelationModels");
            DropIndex("dbo.TestModels", new[] { "RelationId" });
            DropTable("dbo.TestRelationModels");
            DropTable("dbo.TestModels");
        }
    }
}
