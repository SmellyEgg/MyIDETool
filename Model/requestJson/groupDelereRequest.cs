using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class groupDelereRequest
    {
        private string groupid = string.Empty;

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

        public groupDelereRequest(string id)
        {
            this.groupid = id;
        }
    }
}
