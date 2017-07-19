using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class controlUpdateRequest
    {
        public string ctrl_id = string.Empty;

        public string type = string.Empty;

        public fieldObjeForControl field;

        public controlUpdateRequest(string id, string type, fieldObjeForControl obj)
        {
            this.ctrl_id = id;
            this.type = type;
            this.field = obj;
        }
    }

    public class fieldObjeForControl
    {
        public string D2 = string.Empty;

        public string D3 = string.Empty;

        public fieldObjeForControl(string d2, string d3)
        {
            this.D2 = d2;
            this.D3 = d3;
        }
    }
}
