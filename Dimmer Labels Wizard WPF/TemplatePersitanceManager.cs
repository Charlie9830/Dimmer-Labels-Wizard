using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Dimmer_Labels_Wizard_WPF
{
    public class TemplatePersitanceManager
    {
        public TemplatePersitanceManager(string filePath)
        {
            // File.
            _FileStream = new FileStream(filePath, FileMode.OpenOrCreate);

            // Serializer Settings.
            _SerializerSettings = new DataContractSerializerSettings();
            _SerializerSettings.PreserveObjectReferences = true;
            _SerializerSettings.KnownTypes = SerializerKnownTypes.GetTemplateKnownTypes();

            _Serializer = new DataContractSerializer(typeof(List<LabelStripTemplate>), _SerializerSettings);
        }

        protected DataContractSerializerSettings _SerializerSettings;
        protected FileStream _FileStream;
        protected DataContractSerializer _Serializer;


        #region Methods.
        public void SerializeTemplate(LabelStripTemplate template)
        {
            _Serializer.WriteObject(_FileStream, template);
            _FileStream.Close();
        }

        public LabelStripTemplate DeserializeTemplate()
        {
            var template = _Serializer.ReadObject(_FileStream) as LabelStripTemplate;
            _FileStream.Close();

            return template;
        }
        #endregion
    }
}
