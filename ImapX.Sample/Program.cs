using ImapX.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var tmpPath = Path.Combine(Application.StartupPath, "tmp");
            
            try
            {

                try
                {
                    if (Directory.Exists(tmpPath))
                        Directory.Delete(tmpPath, true);
                }
                catch { }

                Directory.CreateDirectory(tmpPath);

                if (args.Any() && args[0].ToLower() == "cmd")
                    Application.Run(new FrmConsole());
                else
                    Application.Run(new FrmMain());
                
            }
            catch(Exception ex)
            {
                
                using(var frm = new FrmError(ex))
                {
                    frm.ShowDialog();
                }

            }
            
        }
    }
}
