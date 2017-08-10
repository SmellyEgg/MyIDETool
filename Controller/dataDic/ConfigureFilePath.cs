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

        /// <summary>
        /// 本地缓存图片
        /// </summary>
        public static string localImageFolder = Application.StartupPath + @"\PageInfo\Image";

        /// <summary>
        /// 已经打开的界面
        /// </summary>
        public static string openedPages = Application.StartupPath + @"\WindowsStatusConfig\pageConfig.xml";

        /// <summary>
        /// 本地组信息
        /// </summary>
        public static string LocalGroupInfo = System.Windows.Forms.Application.StartupPath + "\\GroupConfig\\localGroupInfo.xml";
    }
}
