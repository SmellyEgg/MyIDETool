using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.returnJson
{
    public class pageDetailReturnData
    {
        public string error_code = string.Empty;

        public dataForPageDetail data;
    }

    public class dataForPageDetail
    {
        public string page_id = string.Empty;

        public string page_name = string.Empty;

        public string page_title = string.Empty;

        public string page_model = string.Empty;

        public string page_status = string.Empty;

        public string group_id = string.Empty;

        public string temple_id  = string.Empty;

        public string page_index = string.Empty;

        public string page_level = string.Empty;

        public string page_createtime = string.Empty;

        public string page_width = string.Empty;

        public string page_height = string.Empty;

        public string page_from = string.Empty;

        public string page_key = string.Empty;

        public string page_css = string.Empty;

        public string page_display = string.Empty;

        public string page_background_img_src = string.Empty;

        public string page_background_color = string.Empty;

        public eventListObj event_list;

        public string hidenavigation = string.Empty;

        public object[][] control_list;
    }

    public class eventListObj
    {

    }
}
