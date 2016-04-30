namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Justworkalready : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CellRowTemplateLabelCellTemplates", "CellRowTemplate_ID", "dbo.CellRowTemplates");
            DropForeignKey("dbo.CellRowTemplateLabelCellTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.CellRowTemplateLabelCellTemplates", new[] { "CellRowTemplate_ID" });
            DropIndex("dbo.CellRowTemplateLabelCellTemplates", new[] { "LabelCellTemplate_ID" });
            AddColumn("dbo.CellRowTemplates", "LabelCellTemplate_ID", c => c.Int());
            CreateIndex("dbo.CellRowTemplates", "LabelCellTemplate_ID");
            AddForeignKey("dbo.CellRowTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates", "ID");
            DropTable("dbo.CellRowTemplateLabelCellTemplates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CellRowTemplateLabelCellTemplates",
                c => new
                    {
                        CellRowTemplate_ID = c.Int(nullable: false),
                        LabelCellTemplate_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CellRowTemplate_ID, t.LabelCellTemplate_ID });
            
            DropForeignKey("dbo.CellRowTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.CellRowTemplates", new[] { "LabelCellTemplate_ID" });
            DropColumn("dbo.CellRowTemplates", "LabelCellTemplate_ID");
            CreateIndex("dbo.CellRowTemplateLabelCellTemplates", "LabelCellTemplate_ID");
            CreateIndex("dbo.CellRowTemplateLabelCellTemplates", "CellRowTemplate_ID");
            AddForeignKey("dbo.CellRowTemplateLabelCellTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CellRowTemplateLabelCellTemplates", "CellRowTemplate_ID", "dbo.CellRowTemplates", "ID", cascadeDelete: true);
        }
    }
}
