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
                InstallDirectory = new DirectoryInfo(updateManager.RootAppDirectory);
                var updateInfo = Task.Run(() => updateManager.CheckForUpdate()).Result;
                if (updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Count > 0)
                {
                    var releaseEntry = Task.Run(() => updateManager.UpdateApp()).Result;
                }
            }
        }

        private void CleanUpAppDirectory(string[] names)
        {
            if (names.Length <= 2) { return; }

            for (int i = 0; i < names.Length - 2; i ++)  
            {
                Directory.Delete(names[i], true);
            }
        }

        private void CleanUpPackages(Version latestVersion)
        {
            var packagesDirectory = InstallDirectory.GetDirectories("packages", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (packagesDirectory == null) { return; }

            var packages = packagesDirectory.GetFiles("*.nupkg", SearchOption.TopDirectoryOnly);
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
            InstallDirectory.GetDirectories(SquirrelAppDirectoriesSearchPattern)
                            .ToList()
                            .ForEach(d => { directories.Add(GetVersionFromAppFolder(d), d.FullName); });
            string latestClient = Directory.GetFiles(directories.Last().Value, ClientExeName).FirstOrDefault();

            // Clean up application directories as needed.
            CleanUpAppDirectory(directories.Values.ToArray());

            // Clean up the packages folder as needed.
            CleanUpPackages(directories.Last().Key);

            // Start the client in a new process and kill the bootstrap process.
            Process.Start(latestClient, JsonConvert.SerializeObject(""));
            Process.GetCurrentProcess().Kill();
        }


        private DirectoryInfo InstallDirectory { get; set; }


        private readonly string ReleasesUri = ConfigurationManager.AppSettings["ReleasesUri"];

        private const string ClientExeName = "Client.WinForm.exe";
        private const string SquirrelAppDirectoriesSearchPattern = "app-*";
        private const char SquirrelAppDirectoryDelimiter = '-';
    }
}
