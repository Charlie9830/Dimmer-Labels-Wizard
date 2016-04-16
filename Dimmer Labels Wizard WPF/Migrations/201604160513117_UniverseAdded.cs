namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniverseAdded : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DimmerDistroUnits");
            AddPrimaryKey("dbo.DimmerDistroUnits", new[] { "RackUnitType", "UniverseNumber", "DimmerNumber" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DimmerDistroUnits");
            AddPrimaryKey("dbo.DimmerDistroUnits", new[] { "RackUnitType", "DimmerNumber" });
        }
    }
}
