using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using xinLongIDE.Controller.xml;

namespace xinLongIDE.Controller.CommonController
{
    public class ConnectionController
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        private string _urlService = string.Empty;

        /// <summary>
        /// json业务层
        /// </summary>
        private JsonManager _jsManager;

        public ConnectionController()
        {
            _jsManager = new JsonManager();
        }

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
            request.Timeout = 3000;
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

        /// <summary>
        /// 测试是否能联通服务
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            try
            {
                if (string.IsNullOrEmpty(_urlService))
                {
                    this.GetServiceUrl();
                }
                WebRequest request = (WebRequest)HttpWebRequest.Create(_urlService);
                request.Method = "POST";
                byte[] postBytes = null;
                request.ContentType = @"application/x-www-form-urlencoded";
                postBytes = Encoding.UTF8.GetBytes("");
                request.ContentLength = postBytes.Length;
                request.Timeout = 500;
                using (Stream outstream = request.GetRequestStream())
                {
                }
                return true;
            }
            catch (System.Exception ex)
            {
                string errorCode = "无法连接到远程服务器";
                if (errorCode.Equals(ex.Message))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string PostPhoto(string filepath, KeyValuePair<string, string>[] values)
        {
            if (string.IsNullOrEmpty(_urlService))
            {
                this.GetServiceUrl();
            }
            string url = this._urlService;
            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
            //        var values = new[]
            //        {
            //    new KeyValuePair<string, string>("api_type", "upload"),
            //    new KeyValuePair<string, string>("sql", "select * from hs_new_pages;"),
            //     //other values
            //};

                    foreach (var keyValuePair in values)
                    {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes(filepath)),
                        '"' + "file" + '"',
                        '"' + "defaultImg.png" + '"');

                    var requestUri = url;
                    try
                    {
                        var result = client.PostAsync(requestUri, multipartFormDataContent).Result;
                        return result.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        return string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 调用服务获取返回结果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object GetSqlResult(object obj)
        {
            string jsonText = _jsManager.SerializeToJson(obj);
            string returnStr = this.Post(jsonText);
            return _jsManager.DeSerializeToObject(jsonText);
        }


        public string getReturnStr(object obj)
        {
            string jsonText = _jsManager.SerializeToJson(obj);
            return this.Post(jsonText);
        }
        public void Dispose()
        {
            if (!object.Equals(_jsManager, null))
            {
                _jsManager = null;
            }
        }

    }
}
