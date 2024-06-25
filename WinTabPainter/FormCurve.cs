using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SD = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinTabPainter.Painting;

namespace WinTabPainter
{
    public partial class FormCurve : Form
    {
        public FormCurve()
        {
            InitializeComponent();
        }

        BitmapLayer bitmaplayer;
        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCurve_Load(object sender, EventArgs e)
        {
            var s = new Geometry.Size(this.pictureBox_Curve.Width, this.pictureBox_Curve.Height);
            this.bitmaplayer =new BitmapLayer( s );

        }

        private void FormCurve_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.bitmaplayer!= null)
            {
                this.bitmaplayer.Dispose();
            }
        }
    }
}
