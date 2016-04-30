namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FurtherHouseKeeping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Merges",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VerticalPosition = c.Int(nullable: false),
                        PrimaryUnit_RackUnitType = c.Int(),
                        PrimaryUnit_UniverseNumber = c.Int(),
                        PrimaryUnit_DimmerNumber = c.Int(),
                        Strip_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DimmerDistroUnits", t => new { t.PrimaryUnit_RackUnitType, t.PrimaryUnit_UniverseNumber, t.PrimaryUnit_DimmerNumber })
                .ForeignKey("dbo.Strips", t => t.Strip_ID)
                .Index(t => new { t.PrimaryUnit_RackUnitType, t.PrimaryUnit_UniverseNumber, t.PrimaryUnit_DimmerNumber })
                .Index(t => t.Strip_ID);
            
            AddColumn("dbo.LabelCellTemplates", "IsUniqueTemplate", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabelCellTemplates", "UniqueCellIndex", c => c.Int(nullable: false));
            AddColumn("dbo.LabelCellTemplates", "Strip_ID1", c => c.Int());
            AddColumn("dbo.LabelCellTemplates", "Strip_ID2", c => c.Int());
            AddColumn("dbo.DimmerDistroUnits", "Merge_ID", c => c.Int());
            CreateIndex("dbo.LabelCellTemplates", "Strip_ID1");
            CreateIndex("dbo.LabelCellTemplates", "Strip_ID2");
            CreateIndex("dbo.DimmerDistroUnits", "Merge_ID");
            AddForeignKey("dbo.LabelCellTemplates", "Strip_ID1", "dbo.Strips", "ID");
            AddForeignKey("dbo.DimmerDistroUnits", "Merge_ID", "dbo.Merges", "ID");
            AddForeignKey("dbo.LabelCellTemplates", "Strip_ID2", "dbo.Strips", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabelCellTemplates", "Strip_ID2", "dbo.Strips");
            DropForeignKey("dbo.Merges", "Strip_ID", "dbo.Strips");
            DropForeignKey("dbo.Merges", new[] { "PrimaryUnit_RackUnitType", "PrimaryUnit_UniverseNumber", "PrimaryUnit_DimmerNumber" }, "dbo.DimmerDistroUnits");
            DropForeignKey("dbo.DimmerDistroUnits", "Merge_ID", "dbo.Merges");
            DropForeignKey("dbo.LabelCellTemplates", "Strip_ID1", "dbo.Strips");
            DropIndex("dbo.DimmerDistroUnits", new[] { "Merge_ID" });
            DropIndex("dbo.Merges", new[] { "Strip_ID" });
            DropIndex("dbo.Merges", new[] { "PrimaryUnit_RackUnitType", "PrimaryUnit_UniverseNumber", "PrimaryUnit_DimmerNumber" });
            DropIndex("dbo.LabelCellTemplates", new[] { "Strip_ID2" });
            DropIndex("dbo.LabelCellTemplates", new[] { "Strip_ID1" });
            DropColumn("dbo.DimmerDistroUnits", "Merge_ID");
            DropColumn("dbo.LabelCellTemplates", "Strip_ID2");
            DropColumn("dbo.LabelCellTemplates", "Strip_ID1");
            DropColumn("dbo.LabelCellTemplates", "UniqueCellIndex");
            DropColumn("dbo.LabelCellTemplates", "IsUniqueTemplate");
            DropTable("dbo.Merges");
        }
    }
}
