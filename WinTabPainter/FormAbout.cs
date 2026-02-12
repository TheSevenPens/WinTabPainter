using System;
using System.Text;
using System.Windows.Forms;

namespace WinTabPainter
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.linkLabel_GitHubRepo.Text);
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("WinTabPainter is a Windows application to ");
            sb.AppendLine("serve as a testbed to explore concepts with");
            sb.AppendLine("drawing tablets");
            this.textBox1.Text = sb.ToString();
        }
    }
}
