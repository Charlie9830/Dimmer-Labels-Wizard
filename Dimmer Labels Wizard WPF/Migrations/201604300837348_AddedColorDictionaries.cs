namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColorDictionaries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ColorDictionaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EntriesRackType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ColorEntries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UniverseKey = c.Int(nullable: false),
                        DimmerNumberKey = c.Int(nullable: false),
                        ColorDictionaryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ColorDictionaries", t => t.ColorDictionaryID, cascadeDelete: true)
                .Index(t => t.ColorDictionaryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ColorEntries", "ColorDictionaryID", "dbo.ColorDictionaries");
            DropIndex("dbo.ColorEntries", new[] { "ColorDictionaryID" });
            DropTable("dbo.ColorEntries");
            DropTable("dbo.ColorDictionaries");
        }
    }
}
