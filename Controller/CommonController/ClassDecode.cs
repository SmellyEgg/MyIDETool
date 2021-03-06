﻿using Newtonsoft.Json;
using System.Collections.Generic;
using xinLongIDE.Model.returnJson;

namespace xinLongIDE.Controller.CommonController
{
    /// <summary>
    /// 解析类
    /// </summary>
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

        public CommonReturn DecodegroupCreateReturn(string json)
        {
            CommonReturn m = JsonConvert.DeserializeObject<CommonReturn>(json);
            return m;
        }

        public controlReturnData DecodecontrolReturnData(string json)
        {
            controlReturnData m = JsonConvert.DeserializeObject<controlReturnData>(json);
            return m;
        }

        public pageDetailReturnData DecodepageDetailReturnData(string json)
        {
            pageDetailReturnData m = JsonConvert.DeserializeObject<pageDetailReturnData>(json);
            return m;
        }

        public photoUploadReturnData DecodephotoUploadReturnData(string json)
        {
            photoUploadReturnData m = JsonConvert.DeserializeObject<photoUploadReturnData>(json);
            return m;
        }

        public List<int> DecodeArray(string json)
        {
            List<int> m = JsonConvert.DeserializeObject<List<int>>(json);
            return m;
        }
    }
}
