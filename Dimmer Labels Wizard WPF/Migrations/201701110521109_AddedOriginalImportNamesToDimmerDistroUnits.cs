namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOriginalImportNamesToDimmerDistroUnits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DimmerDistroUnits", "OriginalChannelNumber", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalInstrumentName", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalMulticoreName", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalPosition", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField1", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField2", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField3", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField4", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalCustom", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DimmerDistroUnits", "OriginalCustom");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField4");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField3");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField2");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField1");
            DropColumn("dbo.DimmerDistroUnits", "OriginalPosition");
            DropColumn("dbo.DimmerDistroUnits", "OriginalMulticoreName");
            DropColumn("dbo.DimmerDistroUnits", "OriginalInstrumentName");
            DropColumn("dbo.DimmerDistroUnits", "OriginalChannelNumber");
        }
    }
}
