using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xinLongIDE.Controller.CommonController;

namespace xinLongIDE.Controller.ConfigCMD
{
    public class Function
    {
        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertarrayToString(string[] array)
        {
            StringBuilder str = new StringBuilder(500);
            str.Append("[");
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    str.Append(array[i]);
                }
                else
                {
                    str.Append("," + array[i]);
                }
            }
            str.Append("]");
            return str.ToString();
        }

        /// <summary>
        /// 获取字符串中所有的单词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }

        public static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }

        /// <summary>
        /// 获取转义后的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetTranlatedStr(string text)
        {
            string pattern = @"\.[a-z]\d";
            string input = text.Clone().ToString();
            foreach (Match match in Regex.Matches(input, pattern))
            {
                text = text.Replace(match.Value, "." + I18N.GetContrastTranslatorString(match.Value.Substring(1, match.Value.Length - 1)));
            }
            return text;
        }

        /// <summary>
        /// 获取反转义后的字符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetContrastTranslatedStr(string text)
        {
            string pattern = @"\.\w*[\u4e00-\u9fa5]+";
            string input = text.Clone().ToString();
            foreach (Match match in Regex.Matches(input, pattern))
            {
                text = text.Replace(match.Value, "." + I18N.GetTranslatorString(match.Value.Substring(1, match.Value.Length - 1)));
            }
            return text;
        }

    }
}
