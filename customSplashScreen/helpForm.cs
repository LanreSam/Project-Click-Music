using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace customSplashScreen
{
    public partial class helpForm : Form
    {
        public helpForm()
        {
            InitializeComponent();
        }

        private void website_Click(object sender, EventArgs e)
        {
            var prs = new ProcessStartInfo("chrome.exe");
            prs.Arguments = "https://www.lanresam.ml";
            Process.Start(prs);
        }

        private void twitter_Click(object sender, EventArgs e)
        {
            var prs = new ProcessStartInfo("chrome.exe");
            prs.Arguments = "https://www.twitter.com/lanre_sam";
            Process.Start(prs);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
