using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public static class TemplateHelper
    {
        public static void ModifyExistingTemplate(LabelStripTemplate currentTemplate, LabelStripTemplate newTemplate)
        {
            // Copy Name.
            newTemplate.Name = currentTemplate.Name;

            // Push Template to Assigned Strips, Collect a List of Strips assigned to the currentTemplate. Force immediate
            // execution.
            var assignedStrips = Globals.Strips.Where(item => item.AssignedTemplate == currentTemplate).ToList();

            // Remove the Current Template now. Otherwise, the assign operation below, will make both currentTemplate and
            // newTemplate reference the same object.
            Globals.Templates.Remove(currentTemplate);

            foreach (var element in assignedStrips)
            {
                element.AssignedTemplate = newTemplate;
            }

            // Add the new Template to the Global Collection.
            Globals.Templates.Add(newTemplate);

        }
    }
}
