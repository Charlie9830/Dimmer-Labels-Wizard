using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dimmer_Labels_Wizard
{
    public class Storage
    {
        public void SaveToFile(string fileName)
        {
            GlobalsStorage GlobalsBuffer = new GlobalsStorage();
            UserParametersStorage UserParametersBuffer = new UserParametersStorage();

            // Convert Globals and UserParameters into Storage Objects.
            GlobalsBuffer.DimmerDistroUnits = Globals.DimmerDistroUnits;
            GlobalsBuffer.LabelStrips = Globals.LabelStrips;

            Stream fileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, GlobalsBuffer);
        }

        public void RestoreFromFile(string fileName)
        {
            GlobalsStorage GlobalsBuffer = new GlobalsStorage();
            UserParametersStorage UserParametersBuffer = new UserParametersStorage();

            // DeSerialize File.
            if (File.Exists(fileName))
            {
                Stream fileStream = File.OpenRead(fileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                GlobalsBuffer = (GlobalsStorage)deserializer.Deserialize(fileStream);

                // Assign Data to Globals Static Class.
                Globals.DimmerDistroUnits.Clear();
                Globals.LabelStrips.Clear();

                Globals.DimmerDistroUnits = GlobalsBuffer.DimmerDistroUnits;
                Globals.LabelStrips = GlobalsBuffer.LabelStrips;
            }


        }
    }

    [Serializable]
    public class GlobalsStorage
    {
        public List<DimmerDistroUnit> DimmerDistroUnits = Globals.DimmerDistroUnits;
        public List<LabelStrip> LabelStrips = Globals.LabelStrips;
    }

    [Serializable]
    public class UserParametersStorage
    {

    }
}
