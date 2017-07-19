using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class pageDeleteRequest
    {
        private string pageid = string.Empty;

        public string page_id
        {
            get
            {
                return this.pageid;
            }
            set
            {
                this.pageid = value;
            }
        }

        public pageDeleteRequest(string id)
        {
            this.pageid = id;
        }
    }
}
