namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FurtherAdditionsToUniqueLabelCell : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabelCellTemplates", "IsUniqueEditorDefaultSelection", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LabelCellTemplates", "IsUniqueEditorDefaultSelection");
        }
    }
}
