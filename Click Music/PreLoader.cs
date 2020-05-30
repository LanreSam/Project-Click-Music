using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace customSplashScreen
{
    public partial class PreLoader : Form
    {
        public PreLoader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            loadingPanel.Width += 2;
            //loadingCounter.Text = loadingPanel.Width.ToString();

            if (loadingPanel.Width >= 520)
            {
                timer.Stop();
                Form2 frm = new Form2();
                frm.Show();
                this.Hide();
            }
        }

        private void loadingCounter_Click(object sender, EventArgs e)
        {

        }
    }
}
