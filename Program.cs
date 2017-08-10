using System;
using System.Windows.Forms;

namespace xinLongIDE
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Controller.CommonController.Logging.OpenLogFile();
            Application.Run(new View.MainForm.frmMainIDE());
        }
    }
}
