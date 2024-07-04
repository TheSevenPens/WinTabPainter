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
        [
        Category("Slider"),
        Description("RawMin")
        ]
        public int RawMin
        {
            get => this.trackBar_Raw.Minimum;
            set
            {
                this.trackBar_Raw.Minimum = value;
                this.raw_range = new Numerics.Range(this.RawMin, this.RawMax);
            }
        }

        [
        Category("Slider"),
        Description("RawMax")
        ]
        public int RawMax
        {
            get => this.trackBar_Raw.Maximum;
            set
            {
                this.trackBar_Raw.Maximum = this.raw_range.Clamp(value);
                this.raw_range = new Numerics.Range(this.RawMin, this.RawMax);

            }
        }

        [
        Category("Slider"),
        Description("RawValue")
        ]
        public int RawValue
        {
            get => this.trackBar_Raw.Value;
            set
            {
                this.trackBar_Raw.Value = this.raw_range.Clamp(value);
                this.UpdateNumberFromSlider();
            }
        }

        Numerics.Range raw_range;
        System.Func<int, string> raw_val_to_scaled_string;
        System.Func<string, int?> scaled_string_to_raw_value;

        public ScaledSlider()
        {
            InitializeComponent();
            this.raw_range = new Numerics.Range(this.RawMin, this.RawMax);
            this.raw_val_to_scaled_string = this.RawToScaledString;
            this.scaled_string_to_raw_value = this.ScaledStringToRaw;
            this.UpdateNumberFromSlider();
        }

        public string RawToScaledString(int raw)
        {
            double v = raw / (double)100;
            return v.ToString();
        }

        public int? ScaledStringToRaw(string s)
        {
            double res;
            bool suc = double.TryParse(s, out res);
            if (suc)
            {
                double r = res * 100;
                return (int)r;
            }
            else
            {
                return null;
            }

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
            if (this.raw_val_to_scaled_string != null)
            {
                string s = this.raw_val_to_scaled_string(this.RawValue);
                this.textBoxScaleValue.Text = s;
            }
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
            if (this.scaled_string_to_raw_value != null)
            {
                int? v = this.scaled_string_to_raw_value(this.textBoxScaleValue.Text);
                if (v.HasValue)
                {
                    int v2 = this.raw_range.Clamp(v.Value);
                    this.RawValue = v2;
                    this.UpdateNumberFromSlider();
                }
                else
                {
                    // do nothing
                }

            }

        }

        private void textBoxScaleValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textbox_to_trackbar();
            }
        }
    }
}
