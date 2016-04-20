namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CellRowCollectionChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CellRowTemplates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ManualRowHeight = c.Double(nullable: false),
                        DataField = c.Int(nullable: false),
                        DesiredFontSize = c.Double(nullable: false),
                        LabelCellTemplate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LabelCellTemplates", t => t.LabelCellTemplate_ID)
                .Index(t => t.LabelCellTemplate_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CellRowTemplates", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.CellRowTemplates", new[] { "LabelCellTemplate_ID" });
            DropTable("dbo.CellRowTemplates");
        }
    }
}
