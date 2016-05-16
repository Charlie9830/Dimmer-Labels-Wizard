namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAlphaChannel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ColorEntries", "A", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ColorEntries", "A");
        }
    }
}
