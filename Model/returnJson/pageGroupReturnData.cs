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
        public int unique_id;

        public int group_id = -1;

        public string group_name = string.Empty;

        public pageDetailForGroup[] page_list;
    }

    public class pageDetailForGroup
    {
        public int uniquePageid;
        public int page_id = -1;
        public string page_name = string.Empty;
        //public pageDetailForGroup(int id, string name)
        //{
        //    this.page_id = id;
        //    this.page_name = name;
        //}
    }
}
