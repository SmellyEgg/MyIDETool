using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.Control
{
    /// <summary>
    /// 工具箱项实体
    /// </summary>
    public class toolboxItem
    {
        private string itemText = string.Empty;

        private string controlType = string.Empty;

        private Boolean visible = false;

        /// <summary>
        /// 文本信息
        /// </summary>
        public string ItemText
        {
            get
            {
                return this.itemText;
            }
            set
            {
                this.itemText = value;
            }
        }
        /// <summary>
        /// 控件类型
        /// </summary>
        public string ControlType
        {
            get
            {
                return controlType;
            }

            set
            {
                controlType = value;
            }
        }

        /// <summary>
        /// 是否显性控件
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public toolboxItem(string text,string type, Boolean isVisible)
        {
            this.itemText = text;
            this.controlType = type;
            this.visible = isVisible;
        }
    }
}
