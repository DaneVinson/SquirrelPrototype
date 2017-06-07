using System;
using System.Collections.Generic;
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
            var bootstrapper = new Bootstrapper();
            bootstrapper.CheckAndApplyUpdate();
            bootstrapper.StartClient();
        }
    }
}
