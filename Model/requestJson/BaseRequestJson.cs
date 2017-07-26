using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class BaseRequestJson
    {
        public CommonLoginRequest auth;
        public string api_type = string.Empty;
        public object data;
    }
}
