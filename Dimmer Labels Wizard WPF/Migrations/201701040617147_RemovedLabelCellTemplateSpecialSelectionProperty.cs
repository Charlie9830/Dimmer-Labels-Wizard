namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedLabelCellTemplateSpecialSelectionProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LabelCellTemplates", "IsUniqueEditorDefaultSelection");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LabelCellTemplates", "IsUniqueEditorDefaultSelection", c => c.Boolean(nullable: false));
        }
    }
}
