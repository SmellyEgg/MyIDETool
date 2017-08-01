using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.returnJson
{
    public class pageGroupReturnData
    {
        public pageGroupDetail[] data;
        public string error_code = string.Empty;
    }

    public class pageGroupDetail
    {
        public string group_id = string.Empty;

        public string group_name = string.Empty;

        public pageDetailForGroup[] page_list;
    }

    public class pageDetailForGroup
    {
        public int page_id;
        public string page_name = string.Empty;
        //public pageDetailForGroup(int id, string name)
        //{
        //    this.page_id = id;
        //    this.page_name = name;
        //}
    }
}
