using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.returnJson
{
    public class UserData
    {
        public userConfig data;
        public string error_code = string.Empty;
    }

    public class userConfig
    {
        public string user_id = string.Empty;
        public string user_name = string.Empty;
        public string user_password = string.Empty;
        public string user_company = string.Empty;
        public string user_adress = string.Empty;
        public string user_contact = string.Empty;
        public string user_email = string.Empty;
        public string token = string.Empty;
    }
}
