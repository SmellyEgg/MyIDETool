using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class CommonLoginRequest
    {
        public string user_name = string.Empty;

        public string password = string.Empty;

        public CommonLoginRequest(string name, string pssword)
        {
            this.user_name = name;
            this.password = pssword;
        }
    }
}
