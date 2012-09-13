using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImapX.Sample
{
    static class Program
    {

        public static ImapClient ImapClient;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
            
            Application.Run(new FrmMain());

            Directory.Delete(Path.Combine(Application.StartupPath, "tmp"), true);

        }
    }
}
