using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class groupUpdateRequest
    {
        private string groupid = string.Empty;

        private string groupname = string.Empty;

        public string group_id
        {
            get
            {
                return this.groupid;
            }
            set
            {
                this.groupid = value;
            }
        }

        public string group_name
        {
            get
            {
                return this.groupname;
            }
            set
            {
                this.groupname = value;
            }
        }

        public groupUpdateRequest(string id, string name)
        {
            this.groupid = id;
            this.groupname = name;
        }
    }
}
