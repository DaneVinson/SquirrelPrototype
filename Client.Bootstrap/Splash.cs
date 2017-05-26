using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Bootstrap
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateAvailable = CheckForUpdate();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (UpdateAvailable) { InstallUpdate(); }
            StartClient();
        }


        private bool CheckForUpdate()
        {
            return false;
        }

        private Version GetVersionFromAppFolder(DirectoryInfo directory)
        {
            Version version;
            if (Version.TryParse(directory?.Name.Split(SquirrelAppDirectoryDelimiter).Last(), out version))
            {
                return version;
            }
            else { return new Version(0, 0, 0, 0); }
        }

        private void InstallUpdate()
        {
        }

        private void StartClient()
        {
            // Find the latest client executable
            var directories = new SortedDictionary<Version, string>();
            new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath)
                    .Directory
                    .Parent
                    .GetDirectories(SquirrelAppDirectoriesSearchPattern)
                    .ToList()
                    .ForEach(d => { directories.Add(GetVersionFromAppFolder(d), d.FullName); });
            string latestClient = Directory.GetFiles(directories.Last().Value, ClientExeName).FirstOrDefault();

            // Always show splash for a short time
            var remaining = MinimumSplashTimeSpan - (DateTime.UtcNow - StartTime);
            if (remaining.TotalMilliseconds > 0) { Thread.Sleep(remaining); }

            // Start the client in a new process and kill the bootstrap process.
            Process.Start(latestClient, JsonConvert.SerializeObject(""));
            Process.GetCurrentProcess().Kill();
        }


        private bool UpdateAvailable { get; set; }


        private static readonly TimeSpan MinimumSplashTimeSpan = TimeSpan.FromMilliseconds(500);
        private readonly DateTime StartTime = DateTime.UtcNow;

        private const string ClientExeName = "Client.WinForm.exe";
        private const string SquirrelAppDirectoriesSearchPattern = "app-*";
        private const char SquirrelAppDirectoryDelimiter = '-';
    }
}
