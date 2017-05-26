using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.WinForm
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            theButton.Click += (o, e) => ClickTheButton();
        }

        private void ClickTheButton()
        {
            messageTextBox.Text = Program.JsonOptions;
        }
    }
}
