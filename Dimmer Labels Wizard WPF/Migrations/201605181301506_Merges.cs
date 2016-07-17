namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Merges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Merges", "DimmerDistroUnit_RackUnitType", c => c.Int());
            AddColumn("dbo.Merges", "DimmerDistroUnit_UniverseNumber", c => c.Int());
            AddColumn("dbo.Merges", "DimmerDistroUnit_DimmerNumber", c => c.Int());
            CreateIndex("dbo.Merges", new[] { "DimmerDistroUnit_RackUnitType", "DimmerDistroUnit_UniverseNumber", "DimmerDistroUnit_DimmerNumber" });
            AddForeignKey("dbo.Merges", new[] { "DimmerDistroUnit_RackUnitType", "DimmerDistroUnit_UniverseNumber", "DimmerDistroUnit_DimmerNumber" }, "dbo.DimmerDistroUnits", new[] { "RackUnitType", "UniverseNumber", "DimmerNumber" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Merges", new[] { "DimmerDistroUnit_RackUnitType", "DimmerDistroUnit_UniverseNumber", "DimmerDistroUnit_DimmerNumber" }, "dbo.DimmerDistroUnits");
            DropIndex("dbo.Merges", new[] { "DimmerDistroUnit_RackUnitType", "DimmerDistroUnit_UniverseNumber", "DimmerDistroUnit_DimmerNumber" });
            DropColumn("dbo.Merges", "DimmerDistroUnit_DimmerNumber");
            DropColumn("dbo.Merges", "DimmerDistroUnit_UniverseNumber");
            DropColumn("dbo.Merges", "DimmerDistroUnit_RackUnitType");
        }
    }
}
