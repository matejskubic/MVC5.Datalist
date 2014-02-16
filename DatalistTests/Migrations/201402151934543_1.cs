namespace DatalistTests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Number = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NullableString = c.String(),
                        FirstRelationModelId = c.String(maxLength: 128),
                        SecondRelationModelId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestRelationModels", t => t.FirstRelationModelId)
                .ForeignKey("dbo.TestRelationModels", t => t.SecondRelationModelId)
                .Index(t => t.FirstRelationModelId)
                .Index(t => t.SecondRelationModelId);
            
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
            DropForeignKey("dbo.TestModels", "SecondRelationModelId", "dbo.TestRelationModels");
            DropForeignKey("dbo.TestModels", "FirstRelationModelId", "dbo.TestRelationModels");
            DropIndex("dbo.TestModels", new[] { "SecondRelationModelId" });
            DropIndex("dbo.TestModels", new[] { "FirstRelationModelId" });
            DropTable("dbo.TestRelationModels");
            DropTable("dbo.TestModels");
        }
    }
}
