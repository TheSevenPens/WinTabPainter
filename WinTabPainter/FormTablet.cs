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
    public partial class FormTablet : Form
    {
        public FormTablet()
        {
            InitializeComponent();
        }

        public WinTabUtils.TabletInfo tablet_info;

        private void FormTablet_Load(object sender, EventArgs e)
        {
            this.label_tabletname_val.Text = this.tablet_info.Name;

        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
