namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DimmerDistroUnitHouseKeeping : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DimmerDistroUnits", "OmitUnit");
            DropColumn("dbo.DimmerDistroUnits", "DimmerNumberText");
            DropColumn("dbo.DimmerDistroUnits", "DMXAddressText");
            DropColumn("dbo.DimmerDistroUnits", "AbsoluteDMXAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DimmerDistroUnits", "AbsoluteDMXAddress", c => c.Int(nullable: false));
            AddColumn("dbo.DimmerDistroUnits", "DMXAddressText", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "DimmerNumberText", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OmitUnit", c => c.Boolean(nullable: false));
        }
    }
}
