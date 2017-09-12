using System;
using System.Collections.Generic;
using System.Windows.Forms;
using xinLongIDE.Controller.dataDic;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class toolboxController
    {
        private static List<string> listOfControlTypevalue = new List<string>() {
            xinLongyuControlType.buttonType,
            xinLongyuControlType.textType,
            xinLongyuControlType.imgType,
            xinLongyuControlType.inputType,
            xinLongyuControlType.webviewType,
            xinLongyuControlType.superViewType,
            xinLongyuControlType.listsType,
            xinLongyuControlType.rtfType,//富文本控件
            xinLongyuControlType.tabbarType, //菜单栏控件
            xinLongyuControlType.bannerType,//广告轮播
            xinLongyuControlType.cellType,//单元格控件
            xinLongyuControlType.channerBarType,//频道栏
            xinLongyuControlType.multilineListType,//多行列表
            xinLongyuControlType.sectionType,//行属性
            xinLongyuControlType.radioType,//单选框
            xinLongyuControlType.checkboxType,//复选框
            xinLongyuControlType.selectBoxPopupType,//弹出选择框
            xinLongyuControlType.RatingBarType,//评分控件
            xinLongyuControlType.numSelectorType,//数量选择器
            xinLongyuControlType.dynamicListsType,//动态布局列表
            //xinLongyuControlType.navigationBarType,//导航栏
            //隐性控件
            xinLongyuControlType.timerType,//计时器控件
            xinLongyuControlType.uploadImageType,//图片上传控件
            xinLongyuControlType.jumpPageType,//页面跳转控件
            xinLongyuControlType.tooltipType,//弹窗提示
            xinLongyuControlType.callType,//通话控件
            xinLongyuControlType.locationType,//位置控件
            xinLongyuControlType.cacheType,//本地缓存控件
            xinLongyuControlType.pageCacheType,//页面缓存
            xinLongyuControlType.getDataType,//数据获取
            xinLongyuControlType.triggerType,//触发器
            xinLongyuControlType.refreshType,//刷新
            xinLongyuControlType.QRCodeType,//二维码
            xinLongyuControlType.datePickerType,//日期选择
            xinLongyuControlType.videoType,//视频播放
            xinLongyuControlType.SMSVerificationSendType,//短信发送
            xinLongyuControlType.SMSVerificationVerifyType,//短信验证
            xinLongyuControlType.LogicJudgmentType,//逻辑判断
            xinLongyuControlType.ShareType, //分享控件
            //PC独有的控件
            xinLongyuControlType.GridColumnName,//表格列名控件
            xinLongyuControlType.PCGrid,//PC表格控件
            xinLongyuControlType.switcherType,//开关控件
            xinLongyuControlType.seperateLineType//分隔线
        };

        private static List<Boolean> listOfControlTypeVisibility = new List<Boolean>() {
            true,//按钮
            true,//标签
            true,//图片
            true,//输入框
            true,//浏览器
            true,//父控件
            true,//列表
            true,//富文本控件
            true,//菜单栏控件
            true,//广告轮播
            true,//单元格控件
            true,//频道栏
            true,//多行列表
            true,//行属性
            true,//单选框
            true,//复选框
            true,//弹出选择框
            true,//评分控件
            true,//数量选择器控件
            true, //动态布局列表
            //true,
            //隐性控件
            ////
            ////
            false, //计时器控件
            false,//图片上传控件
            false,//页面跳转控件
            false,//弹窗提示
            false,//通话控件
            false,//位置控件
            false,//本地缓存控件
            false,//页面缓存
            false,//数据获取
            false,//触发器
            false,//刷新
            false,//二维码
            false,//日期选择
            false,//视频播放
            false,//短信发送
            false,//短信验证
            false,//逻辑判断
            false,//分享控件

            true,//PC表格的列名
            true,//PC表格控件
            true,//开关
            true//分隔线
        };

        /// <summary>
        /// 根据控件类型获取显隐性
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        public static bool IsVisible(string ctrlType)
        {
            int index = listOfControlTypevalue.FindIndex(p => ctrlType.ToLower().Equals(p.ToLower()));
            if (index != -1)
            {
                bool bl = listOfControlTypeVisibility[index];
                return bl;
            }
            else
            {
                return false;
            }
        }

        public static string GetControlTypeChineseName(string ctrlType)
        {
            int index = listOfControlTypevalue.FindIndex(p => ctrlType.ToLower().Equals(p.ToLower()));
            if (index != -1)
            {
                
                return ((rowsOfControlList)index).ToString();
            }
            else
            {
                //默认返回图片
                return "图片";
            }
        }
            
        private static ImageList imageListOfControlList = new ImageList();

        private static void LoadImageList()
        {
            imageListOfControlList.Images.Clear();
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuButton);
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuLabel);
            imageListOfControlList.Images.Add(Properties.Resources.defaultImg);
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuWebView);
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuSuperView);
            imageListOfControlList.Images.Add(Properties.Resources.xinLongyuList);
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//菜单栏控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//广告轮播
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//单元格控件 
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//频道栏
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//多行列表
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//行属性
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//单选框
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//复选框
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//弹出选择框
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//评分控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//数量选择器控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//动态布局列表
            //隐性控件
            ////
            ////
            imageListOfControlList.Images.Add(Properties.Resources.xinlongyuTimer);//计时器
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//图片上传控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//页面跳转控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//弹窗提示
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//通话控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//位置控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//本地缓存控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//页面缓存
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//数据获取
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//触发器
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//刷新
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//二维码
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//日期选择
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//视频播放
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//短信发送
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//短信验证
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//逻辑判断
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//分享控件

            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//PC表格列名控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//PC表格控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//PC开关控件
            imageListOfControlList.Images.Add(Properties.Resources.xinglongyuTextbox);//PC分隔线
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
            文本输入框,
            webview,
            父控件,
            列表控件,
            富文本,
            底部菜单栏,
            广告轮播,
            单元格控件,
            频道栏,
            多行列表,
            行属性,
            单选框,
            复选框,
            弹出选择框,
            评分控件,
            数量选择器控件,
            动态布局列表,
            //隐性控件
            ///
            ///
            计时器,
            图片上传控件,
            页面跳转控件,
            弹窗提示,
            通话控件,
            位置控件,
            本地缓存控件,
            页面缓存,
            数据获取,
            触发器,
            刷新,
            二维码,
            日期选择,
            视频播放,
            短信发送,
            短信验证,
            逻辑判断,
            分享控件,
            表格列属性,
            表格控件,
            开关控件,
            分隔线
        }

        
    }
}
