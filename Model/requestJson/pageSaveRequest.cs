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
        public string page_id = string.Empty;
        public pageSaveRequest(string pageName, string pageID)
        {
            this.page = new pageObjForSavePage(pageName);
            this.page_id = pageID;
        }
    }

    public class pageObjForSavePage
    {
        public string page_name = string.Empty;
        public pageObjForSavePage(string name)
        {
            this.page_name = name;
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
