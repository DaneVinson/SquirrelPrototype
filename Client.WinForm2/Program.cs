using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.WinForm2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Task.Run(() => CheckAndApplyUpdate()).Wait();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        public static async Task CheckAndApplyUpdate()
        {
            try
            {
                using (var updateManager = new UpdateManager(ReleasesUri))
                {
                    var updateInfo = await updateManager.CheckForUpdate();
                    if (updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0)
                    {
                        var releaseEntry = await updateManager.UpdateApp();
                        UpdateManager.RestartApp(ClientExeName);
                    }
                }
            }
            catch (WebException exception)
            {
                MessageBox.Show($"WebException likely caused by an invalid URI to Squirrel's Releases. URI value: {ReleasesUri}");
            }
            catch (Exception exception)
            {
                if (exception.Message.ToLower().Contains("squirrel"))
                {
                    MessageBox.Show("Not Squirrel deployed.");
                }
                else { throw; }
            }
        }

        private const string ClientExeName = "Client.WinForm2.exe";
        private static readonly string ReleasesUri = ConfigurationManager.AppSettings["ReleasesUri"];
    }
}
