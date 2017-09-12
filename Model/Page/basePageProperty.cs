namespace xinLongIDE.Model.Page
{
    /// <summary>
    /// 页面属性实体
    /// 用于在画板与属性界面之间的交互
    /// </summary>
    public class basePageProperty
    {
        private string pageName = string.Empty;

        private int pageWidth = 0;

        private int pageHeight = 0;

        private int pageId = 0;

        private string userGroup = string.Empty;

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

        public int PageWidth
        {
            get
            {
                return pageWidth;
            }

            set
            {
                pageWidth = value;
            }
        }

        public int PageHeight
        {
            get
            {
                return pageHeight;
            }

            set
            {
                pageHeight = value;
            }
        }

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

        public string UserGroup
        {
            get
            {
                return userGroup;
            }

            set
            {
                userGroup = value;
            }
        }
    }
}
