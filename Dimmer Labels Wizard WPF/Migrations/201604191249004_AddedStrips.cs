namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStrips : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Strips",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Universe = c.Int(nullable: false),
                        FirstDimmer = c.Int(nullable: false),
                        LastDimmer = c.Int(nullable: false),
                        RackType = c.Int(nullable: false),
                        AssignedTemplate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LabelStripTemplates", t => t.AssignedTemplate_ID)
                .Index(t => t.AssignedTemplate_ID);
            
            AddColumn("dbo.LabelCellTemplates", "Strip_ID", c => c.Int());
            CreateIndex("dbo.LabelCellTemplates", "Strip_ID");
            AddForeignKey("dbo.LabelCellTemplates", "Strip_ID", "dbo.Strips", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Strips", "AssignedTemplate_ID", "dbo.LabelStripTemplates");
            DropForeignKey("dbo.LabelCellTemplates", "Strip_ID", "dbo.Strips");
            DropIndex("dbo.LabelCellTemplates", new[] { "Strip_ID" });
            DropIndex("dbo.Strips", new[] { "AssignedTemplate_ID" });
            DropColumn("dbo.LabelCellTemplates", "Strip_ID");
            DropTable("dbo.Strips");
        }
    }
}
