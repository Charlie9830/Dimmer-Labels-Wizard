namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LabelCellTemplateNav : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CellRowTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.CellRowTemplates", new[] { "LabelCellTemplate_ID" });
            CreateTable(
                "dbo.CellRowTemplateLabelCellTemplates",
                c => new
                    {
                        CellRowTemplate_ID = c.Int(nullable: false),
                        LabelCellTemplate_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CellRowTemplate_ID, t.LabelCellTemplate_ID })
                .ForeignKey("dbo.CellRowTemplates", t => t.CellRowTemplate_ID, cascadeDelete: true)
                .ForeignKey("dbo.LabelCellTemplates", t => t.LabelCellTemplate_ID, cascadeDelete: true)
                .Index(t => t.CellRowTemplate_ID)
                .Index(t => t.LabelCellTemplate_ID);
            
            DropColumn("dbo.CellRowTemplates", "LabelCellTemplate_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CellRowTemplates", "LabelCellTemplate_ID", c => c.Int());
            DropForeignKey("dbo.CellRowTemplateLabelCellTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropForeignKey("dbo.CellRowTemplateLabelCellTemplates", "CellRowTemplate_ID", "dbo.CellRowTemplates");
            DropIndex("dbo.CellRowTemplateLabelCellTemplates", new[] { "LabelCellTemplate_ID" });
            DropIndex("dbo.CellRowTemplateLabelCellTemplates", new[] { "CellRowTemplate_ID" });
            DropTable("dbo.CellRowTemplateLabelCellTemplates");
            CreateIndex("dbo.CellRowTemplates", "LabelCellTemplate_ID");
            AddForeignKey("dbo.CellRowTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates", "ID");
        }
    }
}
