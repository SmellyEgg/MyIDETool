using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller
{
    public class ClassDecode
    {
        public BaseReturnJson DecodeBaseReturnJson(string json)
        {
            BaseReturnJson m = JsonConvert.DeserializeObject<BaseReturnJson>(json);
            return m;
        }

        public UserData DecodeUserData(string json)
        {
            UserData m = JsonConvert.DeserializeObject<UserData>(json);
            return m;
        }

        public pageGroupReturnData DecodepageGroupReturnData(string json)
        {
            pageGroupReturnData m = JsonConvert.DeserializeObject<pageGroupReturnData>(json);
            return m;
        }

        public groupCreateReturn DecodegroupCreateReturn(string json)
        {
            groupCreateReturn m = JsonConvert.DeserializeObject<groupCreateReturn>(json);
            return m;
        }
    }
}
