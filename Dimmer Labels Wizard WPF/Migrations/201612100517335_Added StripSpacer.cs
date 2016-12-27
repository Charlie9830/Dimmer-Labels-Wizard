namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStripSpacer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StripSpacers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Width = c.Double(nullable: false),
                        LabelStripTemplate_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LabelStripTemplates", t => t.LabelStripTemplate_ID)
                .Index(t => t.LabelStripTemplate_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StripSpacers", "LabelStripTemplate_ID", "dbo.LabelStripTemplates");
            DropIndex("dbo.StripSpacers", new[] { "LabelStripTemplate_ID" });
            DropTable("dbo.StripSpacers");
        }
    }
}
