namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdas : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.SerializableFonts");
        }
        
        public override void Down()
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
    }
}
