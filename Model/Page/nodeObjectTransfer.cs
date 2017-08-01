using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.Page
{
    public class nodeObjectTransfer
    {
        private int pageId = -1;

        private string groupId = string.Empty;

        private string pageName = string.Empty;

        /// <summary>
        /// 页面ID
        /// </summary>
        public int PageId
        {
            get
            {
                return pageId;
            }

            set
            {
                pageId = value;
            }
        }

        /// <summary>
        /// 组ID
        /// </summary>
        public string GroupId
        {
            get
            {
                return groupId;
            }

            set
            {
                groupId = value;
            }
        }

        public string PageName
        {
            get
            {
                return pageName;
            }

            set
            {
                pageName = value;
            }
        }
    }
}
