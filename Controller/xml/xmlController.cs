using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

    }
}
