using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimmer_Labels_Wizard_WPF;
using System.Data.Entity;

namespace DatabaseReset
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This Script will attempt to reset the Dimmer Labels Wizard Local Database.");
            Console.WriteLine("THIS WILL DESTROY ANY EXISTING DATA YOU HAVE");
            Console.WriteLine("Press the 'y' key if you wish to continue...");

            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                Console.WriteLine("Attempting to Connect to Dimmer Labels Wizard Database");
                Console.WriteLine("Connection Succsess");

                Console.WriteLine("Resetting Database");
                
                // Drop Create Database.
                Database.SetInitializer(new DropCreateDatabaseAlways<PrimaryDB>());

                Console.WriteLine("Opening a Context to the Database, this will Execute the Database Drop Create Command");

                using (var context = new PrimaryDB())
                {
                    Console.WriteLine("Checking Database Contents (Should be Empty), Please wait, this can take a few momments...");
                    Console.WriteLine("If nothing happens for a minute or so and a sudden wall of text appears talking about an Unexpected Error,");
                    Console.WriteLine("then the script has failed due to a problem I haven't been able to figure out yet. Congradulations! You are one of the 25% ");
                    Console.WriteLine("of computers that have this issue. you can still try the Main Dimmer Labels Wizard Application, but it probalby won't work.");
                    Console.WriteLine(" Sorry mate!");
                    Console.WriteLine("Color Dictionary Count {0}",context.ColorDictionaries.Count());
                    Console.WriteLine("Strips Count {0}",context.Strips.Count());
                    Console.WriteLine("Templates Count {0}",context.Templates.Count());
                    Console.WriteLine("Units Count {0}",context.Units.Count());

                    if (context.ColorDictionaries.Count() == 0 && context.Strips.Count() == 0 && context.Templates.Count() == 0
                        && context.Units.Count() == 0)
                    {
                        Console.WriteLine("Database Reset appears to have been a succsess. Hope you didn't need that data...");
                    }

                    else
                    {
                        Console.WriteLine("Data still exists within the Database... That's interesting. Give the Main Dimmer Labels Wizard Application a go anywhere.. Might work?");
                    }
                }

                Console.WriteLine("Press Any key to Exit");
                Console.Read();
                
            }

            else
            {
                Console.WriteLine();
                Console.WriteLine("Database Reset Cancelled, Press any key to Exit");
                Console.Read();
            }
        }
    }
}
