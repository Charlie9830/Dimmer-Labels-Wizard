using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dimmer_Labels_Wizard_WPF
{
    public class GlobalPersistanceManager
    {
        public GlobalPersistanceManager(string filePath)
        {
            // FileStream.
            _FileStream = new FileStream(filePath, FileMode.OpenOrCreate);

            // Serializer Settings.
            var serializerSettings = new DataContractSerializerSettings();
            serializerSettings.PreserveObjectReferences = true;
            serializerSettings.KnownTypes = SerializerKnownTypes.GetGlobalKnownTypes();
           
            // Serializer.
            _Serializer = new DataContractSerializer(typeof(ProgramState), serializerSettings);
        }

        protected FileStream _FileStream;
        protected DataContractSerializer _Serializer;

        public void SerializeProgramState(ProgramState programState)
        {
            _Serializer.WriteObject(_FileStream, programState);
            _FileStream.Close();
        }

        public ProgramState DeserializeProgramState()
        {
            var programState = _Serializer.ReadObject(_FileStream) as ProgramState;
            _FileStream.Close();

            return programState;
        }
    }
}
