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
            Controller.Logging.OpenLogFile();
            Application.Run(new View.frmMainIDE());
        }
    }
}
