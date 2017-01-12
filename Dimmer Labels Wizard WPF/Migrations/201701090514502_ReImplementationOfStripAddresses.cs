namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReImplementationOfStripAddresses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StripAddresses", "RackUnitType", c => c.Int(nullable: false));
            AddColumn("dbo.StripAddresses", "UniverseNumber", c => c.Int(nullable: false));
            AddColumn("dbo.StripAddresses", "DimmerNumber", c => c.Int(nullable: false));
            DropColumn("dbo.StripAddresses", "HorizontalIndex");
            DropColumn("dbo.StripAddresses", "VerticalPosition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StripAddresses", "VerticalPosition", c => c.Int(nullable: false));
            AddColumn("dbo.StripAddresses", "HorizontalIndex", c => c.Int(nullable: false));
            DropColumn("dbo.StripAddresses", "DimmerNumber");
            DropColumn("dbo.StripAddresses", "UniverseNumber");
            DropColumn("dbo.StripAddresses", "RackUnitType");
        }
    }
}
