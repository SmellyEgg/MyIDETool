using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xinLongIDE.Controller.dataDic;
using xinLongIDE.Model.Setting;

namespace xinLongIDE.Controller.ConfigCMD
{
    /// <summary>
    /// windows窗体位置配置文件
    /// </summary>
    public class windowsStatusController
    {
        /// <summary>
        /// 读配置文件
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<FormStatusSettings> GetFormSettings()
        {
            if (!System.IO.File.Exists(ConfigureFilePath.FormStatusConfigFilePath))
            {
                return null;
            }
            return xmlController.ReadFromXmlFile<List<FormStatusSettings>>(ConfigureFilePath.FormStatusConfigFilePath);
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="list"></param>
        public void SaveFormStatusSettings(List<FormStatusSettings> list)
        {
            if (System.IO.File.Exists(ConfigureFilePath.FormStatusConfigFilePath))
            {
                System.IO.File.Delete(ConfigureFilePath.FormStatusConfigFilePath);
            }

            xmlController.WriteToXmlFile<List<FormStatusSettings>>(ConfigureFilePath.FormStatusConfigFilePath, list);
        }
    }
}
