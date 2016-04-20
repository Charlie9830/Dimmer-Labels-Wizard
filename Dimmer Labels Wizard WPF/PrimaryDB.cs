namespace Dimmer_Labels_Wizard_WPF
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;

    public class PrimaryDB : DbContext
    {
        // Your context has been configured to use a 'PrimaryDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Dimmer_Labels_Wizard_WPF.PrimaryDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'PrimaryDB' 
        // connection string in the application configuration file.
        public PrimaryDB()
            : base("name=PrimaryDB")
        {
        }

        // Units.
        public virtual DbSet<DimmerDistroUnit> Units { get; set; }

        // Templates.
        public virtual DbSet<LabelStripTemplate> Templates { get; set; }

        // Strips.
        public virtual DbSet<Strip> Strips { get; set; }
    }
}