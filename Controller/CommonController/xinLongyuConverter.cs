using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Controller.CommonController
{
    public class xinLongyuConverter
    {
        public static int StringToInt(string str)
        {
            try
            {
                int result = Convert.ToInt32(str);
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static Boolean StringToBool(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            else if ("1".Equals(str) || "True".Equals(str) || "true".Equals(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
