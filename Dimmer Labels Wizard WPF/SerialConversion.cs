using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace Dimmer_Labels_Wizard_WPF
{
    
    public class ApplicationSerialization
    {
        public VersionInfo SaveFileInfo = new VersionInfo("1.2", "-");

        public List<LabelStripStorage> LabelStrips = new List<LabelStripStorage>();
        public List<DimmerDistroUnitStorage> DimmerDistroUnits = new List<DimmerDistroUnitStorage>();

        public UserParametersStorage UserParametersStorage;

        public void PrepareSerialization()
        {

            foreach (var element in Globals.DimmerDistroUnits)
            {
                DimmerDistroUnits.Add(element.GenerateStorage());
            }

            UserParametersStorage = UserParameters.GenerateStorage();
        }
    }


    public class Persistance
    {
        public void SaveToFile(string filePath)
        {
            ApplicationSerialization applicationStorage = new ApplicationSerialization();
            applicationStorage.PrepareSerialization();

            FileStream stream = new FileStream(filePath,FileMode.Create);

            var serializerSettings = new DataContractSerializerSettings();
            serializerSettings.PreserveObjectReferences = true;

            var serializer = new DataContractSerializer(typeof(ApplicationSerialization), serializerSettings);
            serializer.WriteObject(stream, applicationStorage);
            stream.Close();
        }

        public void LoadFromFile(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);

            var serializerSettings = new DataContractSerializerSettings();
            serializerSettings.PreserveObjectReferences = true;

            var deSerializer = new DataContractSerializer(typeof(ApplicationSerialization), serializerSettings);

            var applicationStorage = deSerializer.ReadObject(stream) as ApplicationSerialization;
            UpdateToApplication(applicationStorage);
        }

        public void UpdateToApplication(ApplicationSerialization applicationStorage)
        {
           

            foreach (var element in applicationStorage.DimmerDistroUnits)
            {
                Globals.DimmerDistroUnits.Add(new DimmerDistroUnit(element));
            }

            UserParameters.Rebuild(applicationStorage.UserParametersStorage);
        }
    }

    
    public struct VersionInfo
    {
        public VersionInfo(string versionNumber, string notes)
        {
            VersionNumber = versionNumber;
            Notes = notes;
        }

        string VersionNumber;
        string Notes;
    }
}
