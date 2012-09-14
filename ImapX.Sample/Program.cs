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

            try
            {

                Application.Run(new FrmMain());

            }
            catch(Exception ex)
            {
                
                using(var frm = new FrmError(ex))
                {
                    frm.ShowDialog();
                }

            }
            Directory.Delete(Path.Combine(Application.StartupPath, "tmp"), true);

        }
    }
}
