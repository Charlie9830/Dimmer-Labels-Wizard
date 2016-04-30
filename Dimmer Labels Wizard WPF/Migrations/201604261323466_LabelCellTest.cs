namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabelCellTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabelCellTemplates", "EFTest", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LabelCellTemplates", "EFTest");
        }
    }
}
