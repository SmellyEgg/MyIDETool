namespace xinLongIDE.Model.Page
{
    public class nodeObjectTransfer
    {
        private int pageId = -1;

        private int groupId = -1;

        private string pageName = string.Empty;

        private string userGroup = string.Empty;

        private string plat_form = string.Empty;

        private int pageTag;

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
        public int GroupId
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

        public string Plat_form
        {
            get
            {
                return plat_form;
            }

            set
            {
                plat_form = value;
            }
        }

        /// <summary>
        /// 页面唯一标识
        /// </summary>
        public int PageTag
        {
            get
            {
                return pageTag;
            }

            set
            {
                pageTag = value;
            }
        }
    }
}
