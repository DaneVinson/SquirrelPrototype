using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.WinForm2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            //Task.Run(() => CheckAndApplyUpdate()).Wait();
        }


        public async Task CheckAndApplyUpdate()
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
        private readonly string ReleasesUri = ConfigurationManager.AppSettings["ReleasesUri"];
    }
}
