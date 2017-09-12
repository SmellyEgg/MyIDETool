using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace xinLongIDE.Controller
{
    /// <summary>
    /// xml控制类
    /// </summary>
    public class xmlController
    {
        private string _configFilePath = System.Windows.Forms.Application.StartupPath + @"\xinLongIDE.xml";
        private XmlDocument _xmlController;

        public xmlController()
        {
            _xmlController = new XmlDocument();
            _xmlController.Load(_configFilePath);
        }

        /// <summary>
        /// 根据路径获取配置文件中对应的值
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string GetNodeByXpath(string xpath)
        {
            if (!object.Equals(_xmlController, null))
            {
                XmlNode node = _xmlController.SelectSingleNode(xpath);
                if (node == null)
                {
                    throw new Exception("没有找到配置的节点结点！");
                }
                string result = node.Attributes[0].Value;
                if (!string.IsNullOrEmpty(result)) return result;
            }
            return string.Empty;
        }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (writer = new StreamWriter(filePath, File.Exists(filePath) ? true : false))
                {
                    serializer.Serialize(writer, objectToWrite);
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer.Close();
                }
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (reader = new StreamReader(filePath))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }
        }

    }
}
