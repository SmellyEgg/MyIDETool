using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace xinLongIDE.Controller.CommonController
{
    using System.IO;
    using xinLongIDE.Properties;

    public static class I18N
    {
        private static Dictionary<string, string> _strings = new Dictionary<string, string>();

        public static Dictionary<string, string> _stringsTranslator = new Dictionary<string, string>();

        public static Dictionary<string, string> _stringsContrastTranslator = new Dictionary<string, string>();

        private static void Init(string res, Dictionary<string, string> dic)
        {
            using (var sr = new StringReader(res))
            {
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (line[0] == '#')
                        continue;

                    var pos = line.IndexOf('=');
                    if (pos < 1)
                        continue;
                    
                    if (dic == _stringsContrastTranslator)
                    {
                        dic[line.Substring(pos + 1)] = line.Substring(0, pos);
                    }
                    else
                    {
                        dic[line.Substring(0, pos)] = line.Substring(pos + 1);
                    }
                }
            }
        }

        static I18N()
        {
            Init(Resources.zh_CN, _strings);
            Init(Resources.zh_Translate, _stringsTranslator);
            Init(Resources.zh_Translate, _stringsContrastTranslator);
            //string name = CultureInfo.CurrentCulture.EnglishName;
            //if (name.StartsWith("Chinese", StringComparison.OrdinalIgnoreCase))
            //{
            //    // choose Traditional Chinese only if we get explicit indication
            //    Init(name.Contains("Traditional")
            //        ? Resources.zh_TW
            //        : Resources.zh_CN);
            //}
            //else if (name.StartsWith("Japan", StringComparison.OrdinalIgnoreCase))
            //{
            //    Init(Resources.ja);
            //}
        }

        public static string GetString(string key)
        {
            return _strings.ContainsKey(key)
                ? _strings[key]
                : key;
        }

        public static string GetTranslatorString(string key)
        {
            return _stringsTranslator.ContainsKey(key)
                ? _stringsTranslator[key]
                : key;
        }

        public static string GetContrastTranslatorString(string key)
        {
            return _stringsContrastTranslator.ContainsKey(key)
                ? _stringsContrastTranslator[key]
                : key;
        }
    }
}
