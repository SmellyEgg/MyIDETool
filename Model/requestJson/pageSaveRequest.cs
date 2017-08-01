using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Model.requestJson
{
    public class pageSaveRequest
    {
        public pageObjForSavePage page;
        public ControlDetailForRequest[] ctrls;
        //public int page_id = -1;
        //public pageSaveRequest(string pageName)
        //{
        //    this.page = new pageObjForSavePage(pageName);
        //    //this.page_id = pageID;
        //}
    }

    public class pageObjForSavePage
    {
        public int page_id = -1;

        public string page_name = string.Empty;

        public string page_title = string.Empty;

        public string page_model = string.Empty;

        public string page_status = string.Empty;

        public string group_id = string.Empty;

        public string temple_id = string.Empty;

        public string page_index = string.Empty;

        public string page_level = string.Empty;

        public string page_createtime = string.Empty;

        public string page_width = string.Empty;

        public string page_height = string.Empty;

        public string page_from = string.Empty;

        public string page_key = string.Empty;

        public string page_css = string.Empty;

        public string page_display = string.Empty;

        public string page_background_img_src = string.Empty;

        public string event_list = string.Empty;

        public string hideNavigation = string.Empty;

        public string page_platform = string.Empty;

        public string version = string.Empty;

        public string last_time = string.Empty;

        public string permissions = string.Empty;

        public string is_menu = string.Empty;

        public string menu_data = string.Empty;

        public string review = string.Empty;

        public string user_group = string.Empty;

        public pageObjForSavePage(int id, string name, string groupid)
        {
            this.page_id = id;
            this.page_name = name;
            this.group_id = groupid;
        }
    }

    //public class ctrlObjForSavePage
    //{
    //    public string ctrl_id = string.Empty;
    //    public string ctrl_level = string.Empty;
    //    public string ctrl_type = string.Empty;
    //    public string id = string.Empty; 
    //    public ctrlObjForSavePage(string ctId, string level, string type, string id)
    //    {
    //        this.ctrl_id = ctId;
    //        this.ctrl_level = level;
    //        this.ctrl_type = type;
    //        this.id = id;
    //    }
    //}

}
