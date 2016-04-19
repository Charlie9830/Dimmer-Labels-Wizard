namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTemplates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabelStripTemplates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EditorUpdatesPending = c.Boolean(nullable: false),
                        StripWidth = c.Double(nullable: false),
                        StripHeight = c.Double(nullable: false),
                        StripMode = c.Int(nullable: false),
                        LowerCellTemplate_ID = c.Int(),
                        UpperCellTemplate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LabelCellTemplates", t => t.LowerCellTemplate_ID)
                .ForeignKey("dbo.LabelCellTemplates", t => t.UpperCellTemplate_ID)
                .Index(t => t.LowerCellTemplate_ID)
                .Index(t => t.UpperCellTemplate_ID);
            
            CreateTable(
                "dbo.LabelCellTemplates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RowHeightMode = c.Int(nullable: false),
                        SingleFieldDesiredFontSize = c.Double(nullable: false),
                        SingleFieldDataField = c.Int(nullable: false),
                        CellDataMode = c.Int(nullable: false),
                        LeftWeight = c.Double(nullable: false),
                        TopWeight = c.Double(nullable: false),
                        RightWeight = c.Double(nullable: false),
                        BottomWeight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabelStripTemplates", "UpperCellTemplate_ID", "dbo.LabelCellTemplates");
            DropForeignKey("dbo.LabelStripTemplates", "LowerCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.LabelStripTemplates", new[] { "UpperCellTemplate_ID" });
            DropIndex("dbo.LabelStripTemplates", new[] { "LowerCellTemplate_ID" });
            DropTable("dbo.LabelCellTemplates");
            DropTable("dbo.LabelStripTemplates");
        }
    }
}
