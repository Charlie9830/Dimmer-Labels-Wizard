namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kitchen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabelCellTemplates", "LabelStripTemplate_ID", c => c.Int());
            CreateIndex("dbo.LabelCellTemplates", "LabelStripTemplate_ID");
            AddForeignKey("dbo.LabelCellTemplates", "LabelStripTemplate_ID", "dbo.LabelStripTemplates", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabelCellTemplates", "LabelStripTemplate_ID", "dbo.LabelStripTemplates");
            DropIndex("dbo.LabelCellTemplates", new[] { "LabelStripTemplate_ID" });
            DropColumn("dbo.LabelCellTemplates", "LabelStripTemplate_ID");
        }
    }
}
