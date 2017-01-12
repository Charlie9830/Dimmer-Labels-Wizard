namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReImplementationOfStripAddresses2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StripAddresses", "VerticalPosition", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StripAddresses", "VerticalPosition");
        }
    }
}
