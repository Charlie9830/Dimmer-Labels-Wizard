namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedNamesofNewDimmerDistroUnitProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DimmerDistroUnits", "LastImportedChannelNumber", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedInstrumentName", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedMulticoreName", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedPosition", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedUserField1", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedUserField2", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedUserField3", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedUserField4", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "LastImportedCustom", c => c.String());
            DropColumn("dbo.DimmerDistroUnits", "OriginalChannelNumber");
            DropColumn("dbo.DimmerDistroUnits", "OriginalInstrumentName");
            DropColumn("dbo.DimmerDistroUnits", "OriginalMulticoreName");
            DropColumn("dbo.DimmerDistroUnits", "OriginalPosition");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField1");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField2");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField3");
            DropColumn("dbo.DimmerDistroUnits", "OriginalUserField4");
            DropColumn("dbo.DimmerDistroUnits", "OriginalCustom");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DimmerDistroUnits", "OriginalCustom", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField4", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField3", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField2", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalUserField1", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalPosition", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalMulticoreName", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalInstrumentName", c => c.String());
            AddColumn("dbo.DimmerDistroUnits", "OriginalChannelNumber", c => c.String());
            DropColumn("dbo.DimmerDistroUnits", "LastImportedCustom");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedUserField4");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedUserField3");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedUserField2");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedUserField1");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedPosition");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedMulticoreName");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedInstrumentName");
            DropColumn("dbo.DimmerDistroUnits", "LastImportedChannelNumber");
        }
    }
}
