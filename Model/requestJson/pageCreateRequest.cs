using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    [Serializable]
    public class pageCreateRequest
    {
        private string groupid = string.Empty;

        private string plat_form = string.Empty;

        private string pagename = string.Empty; 

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

        public string platform
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

        public string page_name
        {
            get
            {
                return pagename;
            }

            set
            {
                pagename = value;
            }
        }

        public pageCreateRequest(string id, string plat, string name)
        {
            this.groupid = id;
            this.plat_form = plat;
            this.pagename = name;
        }

        public pageCreateRequest()
        {

        }
    }
}
