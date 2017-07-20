using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class groupCreateRequest
    {
        public string group_name = string.Empty;

        public string platform = string.Empty;

        public groupCreateRequest(string name, string plat)
        {
            this.group_name = name;
            this.platform = plat;
        }

        public groupCreateRequest()
        {
        }
    }
}
