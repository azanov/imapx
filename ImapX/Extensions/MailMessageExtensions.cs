using ImapX.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace ImapX.Extensions
{
    public static class MailMessageExtensions
    {


        public static string ToEml(this MailMessage mailMessage)
        {
            var tmpPath = "";

            using (var watcher = new FileSystemWatcher(System.IO.Path.GetTempPath(), "*.eml"))
            {
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.Created += new FileSystemEventHandler((sndr, fswEvntArgs) => tmpPath = fswEvntArgs.FullPath);

                try
                {
                    watcher.EnableRaisingEvents = true;

                    var client = new SmtpClient("imapx");
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = System.IO.Path.GetTempPath();
                    client.Send(mailMessage);
                    
                    watcher.EnableRaisingEvents = false;

                    if (string.IsNullOrEmpty(tmpPath))
                        throw new OperationFailedException();
                    else
                    {
                        var eml = File.ReadAllText(tmpPath);
                        File.Delete(tmpPath);
                        return eml;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return null;
                }
                finally
                {
                    if (watcher != null) 
                        watcher.EnableRaisingEvents = false;
                }


            }

            
        }

    }
}
