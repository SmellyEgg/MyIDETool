using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xinLongIDE.Controller.xml;

namespace xinLongIDE.Controller
{
    public class ConnectionController
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        private string _urlService = string.Empty;


        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <returns></returns>
        private void GetServiceUrl()
        {
            xmlController xc = new xmlController();
            //这里多了一步加密解密的步骤
            string key = xc.GetNodeByXpath(configManager.keyPath);
            string url = xc.GetNodeByXpath(configManager.urlPath);
            _urlService = SmellyEggCrypt.CryPtService.DESDecrypt(url, key);
        }

        /// <summary>
        /// 传数据
        /// </summary>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        private string Post(string jsonContent)
        {
            if (string.IsNullOrEmpty(_urlService))
            {
                this.GetServiceUrl();
            }
            WebRequest request = (WebRequest)HttpWebRequest.Create(_urlService);
            request.Method = "POST";
            byte[] postBytes = null;
            request.ContentType = @"application/x-www-form-urlencoded";
            postBytes = Encoding.UTF8.GetBytes(jsonContent);
            request.ContentLength = postBytes.Length;
            using (Stream outstream = request.GetRequestStream())
            {
                outstream.Write(postBytes, 0, postBytes.Length);
            }
            string result = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                if (response != null)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                }
            }
            return result;
        }

        public object GetSqlResult(object obj, Type type)
        {
            string jsonText = JsonManager.SerializeToJson(obj);
            string returnStr = this.Post(jsonText);
            return JsonManager.DeSerializeToObject(jsonText, type);
        }

    }
}
