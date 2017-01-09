namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoredUniqueCellTemplates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LabelCellTemplates", "Strip_ID", "dbo.Strips");
            DropForeignKey("dbo.LabelCellTemplates", "Strip_ID1", "dbo.Strips");
            DropForeignKey("dbo.LabelCellTemplates", "Strip_ID2", "dbo.Strips");
            DropIndex("dbo.LabelCellTemplates", new[] { "Strip_ID" });
            DropIndex("dbo.LabelCellTemplates", new[] { "Strip_ID1" });
            DropIndex("dbo.LabelCellTemplates", new[] { "Strip_ID2" });
            CreateTable(
                "dbo.LabelCellTemplateStrips",
                c => new
                    {
                        LabelCellTemplate_ID = c.Int(nullable: false),
                        Strip_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LabelCellTemplate_ID, t.Strip_ID })
                .ForeignKey("dbo.LabelCellTemplates", t => t.LabelCellTemplate_ID, cascadeDelete: true)
                .ForeignKey("dbo.Strips", t => t.Strip_ID, cascadeDelete: true)
                .Index(t => t.LabelCellTemplate_ID)
                .Index(t => t.Strip_ID);
            
            AddColumn("dbo.LabelCellTemplates", "UniqueCellVerticalPosition", c => c.Int(nullable: false));
            AddColumn("dbo.LabelCellTemplates", "UniqueCellName", c => c.String());
            DropColumn("dbo.LabelCellTemplates", "Strip_ID");
            DropColumn("dbo.LabelCellTemplates", "Strip_ID1");
            DropColumn("dbo.LabelCellTemplates", "Strip_ID2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LabelCellTemplates", "Strip_ID2", c => c.Int());
            AddColumn("dbo.LabelCellTemplates", "Strip_ID1", c => c.Int());
            AddColumn("dbo.LabelCellTemplates", "Strip_ID", c => c.Int());
            DropForeignKey("dbo.LabelCellTemplateStrips", "Strip_ID", "dbo.Strips");
            DropForeignKey("dbo.LabelCellTemplateStrips", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.LabelCellTemplateStrips", new[] { "Strip_ID" });
            DropIndex("dbo.LabelCellTemplateStrips", new[] { "LabelCellTemplate_ID" });
            DropColumn("dbo.LabelCellTemplates", "UniqueCellName");
            DropColumn("dbo.LabelCellTemplates", "UniqueCellVerticalPosition");
            DropTable("dbo.LabelCellTemplateStrips");
            CreateIndex("dbo.LabelCellTemplates", "Strip_ID2");
            CreateIndex("dbo.LabelCellTemplates", "Strip_ID1");
            CreateIndex("dbo.LabelCellTemplates", "Strip_ID");
            AddForeignKey("dbo.LabelCellTemplates", "Strip_ID2", "dbo.Strips", "ID");
            AddForeignKey("dbo.LabelCellTemplates", "Strip_ID1", "dbo.Strips", "ID");
            AddForeignKey("dbo.LabelCellTemplates", "Strip_ID", "dbo.Strips", "ID");
        }
    }
}
