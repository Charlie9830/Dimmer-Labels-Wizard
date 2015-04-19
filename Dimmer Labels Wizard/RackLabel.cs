using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dimmer_Labels_Wizard
{
    public class RackLabel
    {
        // Properties.
        public List<HeaderCell> headers = new List<HeaderCell>();
        public List<FooterCell> footers = new List<FooterCell>();


        // Methods.

        
        public void PrintToConsole()
        {
            int count = 0;
            for (int i = 0; i < headers.Count; i++)
            {
                Console.Write(headers[i].data);
                Console.Write(" | ");
                count++;
            }
            Console.Write("    ");
            Console.Write(count);
            count = 0;

            Console.WriteLine();

            for (int i = 0; i < footers.Count; i++)
            {
                Console.Write(footers[i].top_data);
                Console.Write(" | ");
                count++;
            }
            Console.Write("    ");
            Console.Write(count);
        }

    }
}
