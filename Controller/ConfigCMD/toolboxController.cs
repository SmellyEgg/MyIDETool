using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class toolboxController
    {

        private static List<string> listOfControlTypeList = new List<string>() { "按钮", "标签", "图片", "进度条", "文本输入框", "webview" };

        private static ImageList imageListOfControlList = new ImageList();

        private static void LoadImageList()
        {
            imageListOfControlList.Images.Clear();
            imageListOfControlList.Images.Add(Properties.Resources.toolboxIcon);
            imageListOfControlList.Images.Add(Properties.Resources.toolboxIcon);
            imageListOfControlList.Images.Add(Properties.Resources.toolboxIcon);
            imageListOfControlList.Images.Add(Properties.Resources.toolboxIcon);
            imageListOfControlList.Images.Add(Properties.Resources.toolboxIcon);
            imageListOfControlList.Images.Add(Properties.Resources.toolboxIcon);
        }

        public ImageList GetImageIconList()
        {
            if (imageListOfControlList.Images.Count < 1)
            {
                LoadImageList();
            }
            return imageListOfControlList;
        }

        public List<string> GetAllControlType()
        {
            return listOfControlTypeList;
        }

        public enum rowsOfControlList
        {
            按钮,
            标签,
            图片,
            进度条,
            文本输入框,
            webview
        }
    }
}
