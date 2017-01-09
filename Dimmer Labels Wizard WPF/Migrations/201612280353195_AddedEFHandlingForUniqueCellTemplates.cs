namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEFHandlingForUniqueCellTemplates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StripAddresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HorizontalIndex = c.Int(nullable: false),
                        VerticalPosition = c.Int(nullable: false),
                        LabelCellTemplate_ID = c.Int(),
                        Strip_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LabelCellTemplates", t => t.LabelCellTemplate_ID)
                .ForeignKey("dbo.Strips", t => t.Strip_ID)
                .Index(t => t.LabelCellTemplate_ID)
                .Index(t => t.Strip_ID);
            
            DropColumn("dbo.LabelCellTemplates", "UniqueCellIndex");
            DropColumn("dbo.LabelCellTemplates", "UniqueCellVerticalPosition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LabelCellTemplates", "UniqueCellVerticalPosition", c => c.Int(nullable: false));
            AddColumn("dbo.LabelCellTemplates", "UniqueCellIndex", c => c.Int(nullable: false));
            DropForeignKey("dbo.StripAddresses", "Strip_ID", "dbo.Strips");
            DropForeignKey("dbo.StripAddresses", "LabelCellTemplate_ID", "dbo.LabelCellTemplates");
            DropIndex("dbo.StripAddresses", new[] { "Strip_ID" });
            DropIndex("dbo.StripAddresses", new[] { "LabelCellTemplate_ID" });
            DropTable("dbo.StripAddresses");
        }
    }
}
