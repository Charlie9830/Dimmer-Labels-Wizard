namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mergesagain : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DimmerDistroUnits", "Merge_ID", "dbo.Merges");
            DropIndex("dbo.DimmerDistroUnits", new[] { "Merge_ID" });
            CreateTable(
                "dbo.DimmerDistroUnitMerges",
                c => new
                    {
                        DimmerDistroUnit_RackUnitType = c.Int(nullable: false),
                        DimmerDistroUnit_UniverseNumber = c.Int(nullable: false),
                        DimmerDistroUnit_DimmerNumber = c.Int(nullable: false),
                        Merge_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DimmerDistroUnit_RackUnitType, t.DimmerDistroUnit_UniverseNumber, t.DimmerDistroUnit_DimmerNumber, t.Merge_ID })
                .ForeignKey("dbo.DimmerDistroUnits", t => new { t.DimmerDistroUnit_RackUnitType, t.DimmerDistroUnit_UniverseNumber, t.DimmerDistroUnit_DimmerNumber }, cascadeDelete: true)
                .ForeignKey("dbo.Merges", t => t.Merge_ID, cascadeDelete: true)
                .Index(t => new { t.DimmerDistroUnit_RackUnitType, t.DimmerDistroUnit_UniverseNumber, t.DimmerDistroUnit_DimmerNumber })
                .Index(t => t.Merge_ID);
            
            DropColumn("dbo.DimmerDistroUnits", "Merge_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DimmerDistroUnits", "Merge_ID", c => c.Int());
            DropForeignKey("dbo.DimmerDistroUnitMerges", "Merge_ID", "dbo.Merges");
            DropForeignKey("dbo.DimmerDistroUnitMerges", new[] { "DimmerDistroUnit_RackUnitType", "DimmerDistroUnit_UniverseNumber", "DimmerDistroUnit_DimmerNumber" }, "dbo.DimmerDistroUnits");
            DropIndex("dbo.DimmerDistroUnitMerges", new[] { "Merge_ID" });
            DropIndex("dbo.DimmerDistroUnitMerges", new[] { "DimmerDistroUnit_RackUnitType", "DimmerDistroUnit_UniverseNumber", "DimmerDistroUnit_DimmerNumber" });
            DropTable("dbo.DimmerDistroUnitMerges");
            CreateIndex("dbo.DimmerDistroUnits", "Merge_ID");
            AddForeignKey("dbo.DimmerDistroUnits", "Merge_ID", "dbo.Merges", "ID");
        }
    }
}
