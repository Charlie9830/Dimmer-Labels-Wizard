namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsValidOmitUnitAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DimmerDistroUnits", "OmitUnit", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DimmerDistroUnits", "OmitUnit");
        }
    }
}
