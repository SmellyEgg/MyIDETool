using System.Windows.Forms;

namespace xinLongIDE.View.ExtendForm
{
    public partial class frmWaiting : Form
    {
        public frmWaiting()
        {
            InitializeComponent();
        }

        public void SetProgress(int value)
        {
            this.prgTotalprogress.Value = value;
        }
    }
}
