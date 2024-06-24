using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTabPainter
{
    public partial class FormShortcuts : Form
    {
        public FormShortcuts()
        {
            InitializeComponent();
        }

        private void FormShortcuts_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[ - Reduce brush size");
            sb.AppendLine("] - Increase brush size");
            sb.AppendLine("DELETE - Clear canvas");
            this.textBox1.Text = sb.ToString(); 
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
