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
    public partial class ScaledSlider : UserControl
    {
        public ScaledSlider()
        {
            InitializeComponent();
            this.UpdateNumberFromSlider();
        }

        private void textBox_Value_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateNumberFromSlider();
        }

        private void UpdateNumberFromSlider()
        {
            this.textBox_Value.Text = this.trackBar_Raw.Value.ToString();
        }

        private void textBox_Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textbox_to_trackbar();
            }
        }

        private void textbox_to_trackbar()
        {
            int value = 0;
            bool suc = int.TryParse(this.textBox_Value.Text, out value);
            if (suc)
            {
                var r = new Numerics.Range(this.trackBar_Raw.Minimum, this.trackBar_Raw.Maximum);
                var clamped_value = r.Clamp(value);
                this.trackBar_Raw.Value = r.Clamp(clamped_value);
                this.textBox_Value.Text = clamped_value.ToString();
            }
            else
            {
                this.textBox_Value.Text = this.trackBar_Raw.Value.ToString();
                // coudl not parse value
                // do nothing
            }
        }
    }
}
