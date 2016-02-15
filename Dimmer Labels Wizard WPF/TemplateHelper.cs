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
            if (currentTemplate == null || newTemplate == null)
            {
                return;
            }

            int currentTemplateIndex = Globals.Templates.IndexOf(currentTemplate);

            if (currentTemplateIndex == -1)
            {
                // currentTemplate was not found in Globals.Templates collection.
                return;
            }

            // Copy Name.
            newTemplate.Name = currentTemplate.Name;

            // Collect a List of Strips assigned to the currentTemplate. Force immediate
            // execution.
            var assignedStrips = Globals.Strips.Where(item => item.AssignedTemplate == currentTemplate).ToList();

            // Replace the Current Template now. Otherwise, the assign operation below, will make both currentTemplate and
            // newTemplate reference the same object.
            Globals.Templates[currentTemplateIndex] = newTemplate;

            // Update AssignedStrips.
            foreach (var element in assignedStrips)
            {
                element.AssignedTemplate = newTemplate;
            }
        }

        public static void RemoveExistingTemplate(LabelStripTemplate template)
        {
            if (template == null || !Globals.Templates.Contains(template))
            {
                return;
            }

            // Collect a List of Strips assigned to the currentTemplate. Force immediate
            // execution.
            var assignedStrips = Globals.Strips.Where(item => item.AssignedTemplate == template).ToList();

            // Set AssignedStrips to Default Template.
            foreach (var element in assignedStrips)
            {
                element.AssignedTemplate = Globals.DefaultTemplate;
            }

            // Remove template from Collection.
            Globals.Templates.Remove(template);
        }
    }
}
