using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class photoUploadRequest
    {
        public string file = string.Empty;

        public string sql = string.Empty;


        public photoUploadRequest(string file, string sql)
        {
            this.file = file;
            this.sql = sql;
        }
    }

}
