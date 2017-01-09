using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class ExtensionMethods
    {
        public static string ValidateAndAppendName(this string desiredName, IEnumerable<string> existingNames)
        {
            if (existingNames != null)
            {
                // Find existing Identical Names.
                var existingIdenticalNames = (from name in existingNames
                                              where name == desiredName
                                              select name).ToList();

                if (existingIdenticalNames.Count > 0)
                {
                    // Existing Name found.
                    string existingName = existingIdenticalNames.First();

                    if (existingName == string.Empty)
                    {
                        return ValidateAndAppendName("Imported Template", existingNames);
                    }

                    char lastChar = existingName.ToArray().Last();
                    if (char.IsNumber(lastChar) == true)
                    {
                        // Number has already been Appended, Iterate it.
                        int number = int.Parse(lastChar.ToString());
                        number++;
                        return ValidateAndAppendName(desiredName + " " + number, existingNames);
                    }

                    else
                    {

                        return ValidateAndAppendName(desiredName + " " + 1, existingNames);
                    }
                }

                else
                {
                    // No Existing name Found.
                    return desiredName;
                }
            }

            return desiredName;
        }
    }
}
