using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.WinForm3
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            label1.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var updated = Task.Run(() => CheckAndApplyUpdate()).Result;
            if (updated) { UpdateManager.RestartApp(ClientExeName); }
        }

        public async Task<bool> CheckAndApplyUpdate()
        {
            bool updated = false;
            using (var updateManager = new UpdateManager(ReleasesUri))
            {
                var updateInfo = await updateManager.CheckForUpdate();
                if (updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0)
                {
                    var releaseEntry = await updateManager.UpdateApp();
                    updated = true;
                }
            }
            return updated;
        }


        private const string ClientExeName = "Client.WinForm3.exe";
        private readonly string ReleasesUri = ConfigurationManager.AppSettings["ReleasesUri"];
    }
}
