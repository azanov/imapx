using System;
using System.Windows.Forms;

namespace ImapX.Sample
{
    static class Program
    {
        public static ImapClient Client;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());

        }
    }
}
