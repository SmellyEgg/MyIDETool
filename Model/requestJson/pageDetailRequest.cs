using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class pageDetailRequest
    {
        public int page_id = 0;

        public int time = 0;

        public pageDetailRequest(int id, int time)
        {
            this.page_id = id;
            this.time = time;
        }


    }
}
