using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Controller
{
    /// <summary>
    /// json管理类
    /// </summary>
    public class JsonManager
    {
        /// <summary>
        /// 将类转换成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public string SerializeToJson<T>(T objectToSerialize) where T : class
        {
            if (objectToSerialize == null)
            {
                return string.Empty;
            }
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize);
            return output; 
        }

        /// <summary>
        /// 将json字符串转换为类实体
        /// </summary>
        /// <param name="jsontext"></param>
        /// <returns></returns>
        public object DeSerializeToObject(string jsontext) 
        {
            if (string.IsNullOrEmpty(jsontext))
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsontext);
        }

    }
}
