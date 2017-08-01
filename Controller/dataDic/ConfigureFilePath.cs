using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xinLongIDE.Controller.dataDic
{
    public class ConfigureFilePath
    {
        /// <summary>
        /// 页面位置状态等的配置文件
        /// </summary>
        public static string FormStatusConfigFilePath = Application.StartupPath + @"\WindowsStatusConfig\formConfig.xml";

        /// <summary>
        /// 页面细节配置文件路径文件夹
        /// </summary>
        public static string PageDetailFolder = Application.StartupPath + @"\PageInfo";
    }
}
