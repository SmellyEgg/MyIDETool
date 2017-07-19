using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using xinLongIDE.Model.requestJson;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class pageConfigCMD
    {
        private string addGroupConfigFilePath = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\addGroupConfigFile.xml";

        private string addPageConfigFilePath = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\addPageConfigFile.xml";

        public void AddGroup(string groupName, string platForm)
        {
            groupCreateRequest obj = new groupCreateRequest(groupName, platForm);

            //SaveConfig(obj, addGroupConfigFilePath);
        }

        public void AddPage(string pageName, string groupId, string platForm)
        {
            pageCreateRequest obj = new pageCreateRequest(groupId, platForm, pageName);

            //SaveConfig(obj, addGroupConfigFilePath);
            //WriteToBinaryFile(addPageConfigFilePath, obj);
            //WriteToXmlFile(addPageConfigFilePath, obj);
            WriteToXmlFile<pageCreateRequest>(addPageConfigFilePath, obj);
        }

        private void WriteToBinaryFile<T>(string filePath, T objectToWrite)
        {
            using (Stream stream = File.Open(filePath, File.Exists(filePath) ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public void WriteToXmlFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, File.Exists(filePath)? true:false);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

    }
}
