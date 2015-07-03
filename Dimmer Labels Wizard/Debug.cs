using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard
{
    public static class Debug
    {
        public static void PrintDimDistroToConsole()
        {
            for (int i = 0; i < Globals.DimmerDistroUnits.Count; i++)
            {
                Console.WriteLine("Channel:");
                Console.Write("    ");
                Console.Write(Globals.DimmerDistroUnits[i].ChannelNumber);
                Console.WriteLine();

                Console.WriteLine("Dimmer:");
                Console.Write("    ");
                Console.Write(Globals.DimmerDistroUnits[i].DimmerNumberText);
                Console.WriteLine();

                Console.WriteLine("Instrument Type:");
                Console.Write("    ");
                Console.Write(Globals.DimmerDistroUnits[i].InstrumentName);
                Console.WriteLine();

                Console.WriteLine("Multicore Name:");
                Console.Write("    ");
                Console.Write(Globals.DimmerDistroUnits[i].MulticoreName);
                Console.WriteLine();

                Console.WriteLine("RackUnit Type");
                Console.Write("    ");
                Console.Write(Globals.DimmerDistroUnits[i].RackUnitType);
                Console.WriteLine();

                Console.WriteLine("Rack Number");
                Console.Write("    ");
                Console.Write(Globals.DimmerDistroUnits[i].RackNumber);
                Console.WriteLine();

                Console.WriteLine("===================");

                Console.WriteLine();
            };
        }

        public static void PrintListOrderToConsole()
        {
            foreach (var element in Globals.DimmerDistroUnits)
            {
                if (element.RackUnitType == RackType.Dimmer)
                {
                    Console.WriteLine("Rack {0}  Dimmer: {1} / {2}  Index [{3}]", element.RackNumber, element.UniverseNumber, element.DimmerNumber,Globals.DimmerDistroUnits.IndexOf(element));
                }

                else if (element.RackUnitType == RackType.Distro)
                {
                   Console.WriteLine("Rack {0}  ND: {1} Index [{2}]", element.RackNumber, element.DimmerNumber,Globals.DimmerDistroUnits.IndexOf(element));
                }

                else
                {
                    Console.WriteLine("Rack {0}  Global DMX: {1}", element.RackNumber, element.DimmerNumber);
                }
                
                
            }
        }

        public static void PrintRackLabelsToConsole()
        {
            foreach (var element in Globals.LabelStrips)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------");

                element.PrintToConsole();
                
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine();
                
               
            }
        }

    }
}
