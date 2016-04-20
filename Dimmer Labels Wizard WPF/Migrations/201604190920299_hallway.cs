namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hallway : DbMigration
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
            
            AddColumn("dbo.LabelCellTemplates", "SingleFieldSerializableFont_ID", c => c.Int());
            AddColumn("dbo.CellRowTemplates", "SerializableFont_ID", c => c.Int());
            CreateIndex("dbo.LabelCellTemplates", "SingleFieldSerializableFont_ID");
            CreateIndex("dbo.CellRowTemplates", "SerializableFont_ID");
            AddForeignKey("dbo.CellRowTemplates", "SerializableFont_ID", "dbo.SerializableFonts", "ID");
            AddForeignKey("dbo.LabelCellTemplates", "SingleFieldSerializableFont_ID", "dbo.SerializableFonts", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabelCellTemplates", "SingleFieldSerializableFont_ID", "dbo.SerializableFonts");
            DropForeignKey("dbo.CellRowTemplates", "SerializableFont_ID", "dbo.SerializableFonts");
            DropIndex("dbo.CellRowTemplates", new[] { "SerializableFont_ID" });
            DropIndex("dbo.LabelCellTemplates", new[] { "SingleFieldSerializableFont_ID" });
            DropColumn("dbo.CellRowTemplates", "SerializableFont_ID");
            DropColumn("dbo.LabelCellTemplates", "SingleFieldSerializableFont_ID");
            DropTable("dbo.SerializableFonts");
        }
    }
}
