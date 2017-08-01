using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xinLongIDE.View.Control
{
    public class xinLongyuCommonControl : Panel
    {
        private string text = string.Empty;

        public string TextForShow
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}
