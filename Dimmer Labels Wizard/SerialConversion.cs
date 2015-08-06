using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Dimmer_Labels_Wizard
{
    [Serializable()]
    public class ApplicationSerialization
    {
        public List<LabelStripStorage> LabelStrips = new List<LabelStripStorage>();
        public List<DimmerDistroUnitStorage> DimmerDistroUnits = new List<DimmerDistroUnitStorage>();

        public UserParametersStorage UserParametersStorage;

        public void PrepareSerialization()
        {
            foreach (var element in Globals.LabelStrips)
            {
                LabelStrips.Add(element.GenerateStorage());
            }

            foreach (var element in Globals.DimmerDistroUnits)
            {
                DimmerDistroUnits.Add(element.GenerateStorage());
            }

            UserParametersStorage = UserParameters.GenerateStorage();
        }


    }


    public class Persistance
    {
        public void SaveToFile()
        {
            ApplicationSerialization applicationStorage = new ApplicationSerialization();
            applicationStorage.PrepareSerialization();
            
            string filePath = @"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\SaveFile.dlw";
            FileStream fileStream = File.Create(filePath);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, applicationStorage);
            fileStream.Close();
        }

        public void LoadFromFile()
        {
            ApplicationSerialization applicationStorage = new ApplicationSerialization();
            string filePath = @"C:\Users\Charlie Samsung\SkyDrive\C# Projects\Dimmer Labels Wizard\Test Input Files\SaveFile.dlw";
            FileStream fileStream = File.OpenRead(filePath);
            BinaryFormatter deSerializer = new BinaryFormatter();
            applicationStorage = (ApplicationSerialization)deSerializer.Deserialize(fileStream);
            UpdateToApplication(applicationStorage);

            Console.WriteLine("DeSerialization Complete");
        }

        public void UpdateToApplication(ApplicationSerialization applicationStorage)
        {
            Globals.LabelStrips.Clear();

            foreach (var element in applicationStorage.LabelStrips)
            {
                Globals.LabelStrips.Add(new LabelStrip(element));
            }

            foreach (var element in applicationStorage.DimmerDistroUnits)
            {
                Globals.DimmerDistroUnits.Add(new DimmerDistroUnit(element));
            }

            // Rebuild LabelCell Previous Reference Somehow.

            UserParameters.Rebuild(applicationStorage.UserParametersStorage);
        }
    }
}
