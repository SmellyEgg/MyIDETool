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
        public static string SerializeToJson<T>(T objectToSerialize) where T : class
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentException("objectToSerialize must not be null");
            }
            MemoryStream ms = null;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
                ms = new MemoryStream();
                serializer.WriteObject(ms, objectToSerialize);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }

            //return Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize);
        }

        /// <summary>
        /// 将json字符串转换为类实体
        /// </summary>
        /// <param name="jsontext"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeSerializeToObject(string jsontext, Type type) 
        {
            if (string.IsNullOrEmpty(jsontext))
            {
                throw new ArgumentException("jsontext is empty or null!");
            }
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsontext)))
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(type);
                object obj = deserializer.ReadObject(ms);
                return obj;
            }
        }

    }
}
