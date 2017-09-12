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
