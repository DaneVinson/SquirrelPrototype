using Newtonsoft.Json;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Bootstrap
{
    internal class Bootstrapper
    {
        public void CheckAndApplyUpdate()
        {
            using (var updateManager = new UpdateManager(ReleasesUri))
            {
                var updateInfo = Task.Run(() => updateManager.CheckForUpdate()).Result;
                if (updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0)
                {
                    var releaseEntry = Task.Run(() => updateManager.UpdateApp()).Result;
                }
            }
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

        public void StartClient()
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
        private readonly string ReleasesUri = ConfigurationManager.AppSettings["ReleasesUri"];
        private readonly DateTime StartTime = DateTime.UtcNow;

        private const string ClientExeName = "Client.WinForm.exe";
        private const string SquirrelAppDirectoriesSearchPattern = "app-*";
        private const char SquirrelAppDirectoryDelimiter = '-';
    }
}
