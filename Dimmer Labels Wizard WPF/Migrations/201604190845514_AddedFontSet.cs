namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFontSet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SerializableFonts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsBold = c.Boolean(nullable: false),
                        IsItalics = c.Boolean(nullable: false),
                        IsUnderline = c.Boolean(nullable: false),
                        FontFamilyString = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SerializableFonts");
        }
    }
}
