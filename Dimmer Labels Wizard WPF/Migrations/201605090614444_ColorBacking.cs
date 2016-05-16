namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColorBacking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ColorEntries", "R", c => c.Byte(nullable: false));
            AddColumn("dbo.ColorEntries", "G", c => c.Byte(nullable: false));
            AddColumn("dbo.ColorEntries", "B", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ColorEntries", "B");
            DropColumn("dbo.ColorEntries", "G");
            DropColumn("dbo.ColorEntries", "R");
        }
    }
}
