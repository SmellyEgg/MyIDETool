using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xinLongIDE.Controller.dataDic;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class toolboxController
    {
        private static List<string> listOfControlTypevalue = new List<string>() { xinLongyuControlType.buttonType, xinLongyuControlType.textType, xinLongyuControlType.imgType, xinLongyuControlType.imgType, xinLongyuControlType.inputType, xinLongyuControlType.webviewType, xinLongyuControlType.timerType };

        private static List<Boolean> listOfControlTypeVisibility = new List<Boolean>() { true, true, true, true, true, true, false };

        private static ImageList imageListOfControlList = new ImageList();

        private static void LoadImageList()
        {
            imageListOfControlList.Images.Clear();
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuButton);
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuLabel);
            imageListOfControlList.Images.Add(Properties.Resources.defaultImg);
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuPrgBar);
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuWebView);
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuTimer);
        }

        public ImageList GetImageIconList()
        {
            if (imageListOfControlList.Images.Count < 1)
            {
                LoadImageList();
            }
            return imageListOfControlList;
        }

        public List<string> GetAllControlTypeValue()
        {
            return listOfControlTypevalue;
        }

        public List<Boolean> GetALlControlTypeVisibility()
        {
            return listOfControlTypeVisibility;
        }

        public enum rowsOfControlList
        {
            按钮,
            标签,
            图片,
            进度条,
            文本输入框,
            webview,
            计时器
        }

        
    }
}
