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

        public toolboxItem(string text)
        {
            this.itemText = text;
        }
    }
}
