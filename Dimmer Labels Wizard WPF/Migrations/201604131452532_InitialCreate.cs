namespace Dimmer_Labels_Wizard_WPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DimmerDistroUnits",
                c => new
                    {
                        RackUnitType = c.Int(nullable: false),
                        DimmerNumber = c.Int(nullable: false),
                        ChannelNumber = c.String(),
                        InstrumentName = c.String(),
                        MulticoreName = c.String(),
                        Position = c.String(),
                        UserField1 = c.String(),
                        UserField2 = c.String(),
                        UserField3 = c.String(),
                        UserField4 = c.String(),
                        Custom = c.String(),
                        DimmerNumberText = c.String(),
                        DMXAddressText = c.String(),
                        UniverseNumber = c.Int(nullable: false),
                        AbsoluteDMXAddress = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RackUnitType, t.DimmerNumber });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DimmerDistroUnits");
        }
    }
}
