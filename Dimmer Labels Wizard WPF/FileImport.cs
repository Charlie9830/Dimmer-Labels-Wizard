using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVRead = Microsoft.VisualBasic.FileIO;


namespace Dimmer_Labels_Wizard_WPF
{
    public static class FileImport
    {
        public static bool ValidateFile(string filePath, out string errorMessage)
        {
            CSVRead.TextFieldParser file = CreateTextFieldParser(filePath);
            file.SetDelimiters(",");

            try
            {
                while (!file.EndOfData)
                {
                    file.ReadLine();
                }
            }

            catch (CSVRead.MalformedLineException e)
            {
                errorMessage = e.Message;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }


        public static IEnumerable<ColumnHeader> CollectHeaders(string filePath)
        {
            // Create new CSV object Pointed to File Location.
            CSVRead.TextFieldParser file = CreateTextFieldParser(filePath);
            file.SetDelimiters(",");


            // Read the First line to Collect the Cells.
            string[] headerNames = file.ReadFields();

            // Close the File to return Cursor to Top.
            file.Close();

            // Process headerNames into ColumnHeader objects.
            List<ColumnHeader> columnHeaders = new List<ColumnHeader>();

            int index = 0;
            foreach (var element in headerNames)
            {
                columnHeaders.Add(new ColumnHeader(element, index));
                index++;
            }

            return columnHeaders as IEnumerable<ColumnHeader>;
            
        }

        public static CSVRead.TextFieldParser CreateTextFieldParser(string filePath)
        {
            var textFieldParser = new CSVRead.TextFieldParser(filePath);
            textFieldParser.SetDelimiters(",");

            return textFieldParser;
        }
    }
}