using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.returnJson
{
    public class photoUploadReturnData
    {
        public string error_code = string.Empty;

        public dataObjForPhotoUploadReturnData data;

    }

    public class dataObjForPhotoUploadReturnData
    {
        public string path = string.Empty;

    }
}
