using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Bootstrap
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Debugger.Launch();
            var bootstrapper = new Bootstrapper();
            bootstrapper.CheckAndApplyUpdate();
            bootstrapper.StartClient();
        }
    }
}
