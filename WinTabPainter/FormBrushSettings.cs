using SD = System.Drawing;
using System.Windows.Forms;
using WinTabPainter.Painting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using System.Linq;


namespace WinTabPainter
{

    public partial class FormBrushSettings : Form
    {

        public FormBrushSettings(double amt)
        {
            InitializeComponent();
            this.curve = new Numerics.SimpleCurve();
            this.curve.CurveAmount = amt;





        }
        Numerics.SimpleCurve smoothing_adjustment_curve = new Numerics.SimpleCurve(0.9);

        Painting.BitmapLayer bitmaplayer;
        SD.Pen curve_pen;
        SD.Pen frame_pen;
        SD.PointF[] points;
        Numerics.SimpleCurve curve;
        SD.SolidBrush brush;
        int padding = 25;
        int num_points = 300;

        static int state_pressure_smoothing_trackbar_value = 0;
        static int state_position_smoothing_trackbar_value = 0;

        public double PressureSmoothingValue = 0;
        public double PositionSmoothingValue = 0;
        public bool AntiAliasing;
        int QuantizeLevels;

        public double CurveAmount
        {
            get => this.curve.CurveAmount;
            set => this.curve.CurveAmount = value;
        }

        private void button_Close_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void FormCurve_Load(object sender, System.EventArgs e)
        {
            var s = new Geometry.Size(num_points + 2 * this.padding, num_points + 2 * this.padding);
            this.bitmaplayer = new Painting.BitmapLayer(s);
            this.pictureBox_Curve.Image = this.bitmaplayer.Bitmap;
            this.brush = new SD.SolidBrush(SD.Color.White);
            this.points = new SD.PointF[num_points];
            this.labelAmount.Text = this.curve.CurveAmount.ToString();
            this.curve_pen = new SD.Pen(SD.Color.CornflowerBlue, 5);
            this.frame_pen = new SD.Pen(SD.Color.Gray, 1);
            this.checkBoxAntiAliasing.Checked = this.AntiAliasing;
            this.render_curve();

            var curve_slide_range = new Numerics.OrderedRangeD(-100.0, 100.0);
            var slider_value = (int)curve_slide_range.Clamp(this.curve.CurveAmount * 100.0);
            this.trackBar_Amount.Value = slider_value;

            this.trackBar_PositionSmoothing.Value = state_position_smoothing_trackbar_value;
            this.trackBar_PressureSmoothing.Value = state_pressure_smoothing_trackbar_value;

            update_smoothing_ui(this.trackBar_PositionSmoothing, label_position_smoothingval, out this.PositionSmoothingValue);
            update_smoothing_ui(this.trackBar_PressureSmoothing, label_pressure_smoothingval, out this.PressureSmoothingValue);


            var l = new System.Collections.Generic.List<QuantItem>();

            l.Add(new QuantItem("No Quantization", -1));
            l.Add(new QuantItem("2 levels", 2));
            l.Add(new QuantItem("4 levels", 4));
            l.Add(new QuantItem("8 levels", 8));
            l.Add(new QuantItem("16 levels", 16));
            l.Add(new QuantItem("32 levels", 32));
            l.Add(new QuantItem("64 levels", 64));
            l.Add(new QuantItem("128 levels", 128));
            l.Add(new QuantItem("256 levels", 256));
            l.Add(new QuantItem("512 levels", 512));
            l.Add(new QuantItem("1024 levels", 1024));
            l.Add(new QuantItem("2048 levels", 2048));
            l.Add(new QuantItem("4096 levels", 4096));
            l.Add(new QuantItem("8192 levels", 8192));
            this.comboBox_PressureQuant.DataSource = l;
            this.comboBox_PressureQuant.DisplayMember = "Key";
            this.comboBox_PressureQuant.ValueMember = "Value";
            //this.comboBox_PressureQuant.DataBindings.Add("SelectedItem", l, "Key");

            foreach (var (qitem, index) in l.Select((i, q) => (i, q)))
            {
                if (qitem.Value == this.QuantizeLevels)
                {
                    this.comboBox_PressureQuant.SelectedIndex = index; break;
                }
            }
        }

        private void render_curve()
        {
            int i_max = num_points - 1;


            var x_coord_range = new Numerics.OrderedRangeD(0, i_max);
            var y_coord_range = new Numerics.OrderedRangeD(0, i_max);

            for (int i = 0; i <= i_max; i++)
            {
                double x = i / (double)i_max;
                double y = curve.ApplyCurve(x);

                double x_coord = x_coord_range.Clamp(x * i_max);
                double y_coord = i_max - y_coord_range.Clamp(y * i_max);

                var p = new Geometry.PointD(x_coord, y_coord);
                var p2 = p.Add(this.padding, this.padding);
                var p3 = new SD.PointF((float)p2.X, (float)p2.Y);
                this.points[i] = p3;



            }
            var inner_rect = new SD.Rectangle(this.padding, this.padding, this.bitmaplayer.Width - (2 * this.padding), this.bitmaplayer.Height - (2 * this.padding));
            this.bitmaplayer.Graphics.FillRectangle(this.brush, new SD.Rectangle(0, 0, this.bitmaplayer.Width, this.bitmaplayer.Height));

            this.bitmaplayer.Graphics.DrawRectangle(this.frame_pen, inner_rect);
            this.bitmaplayer.Graphics.DrawLines(this.curve_pen, this.points);
            this.pictureBox_Curve.Invalidate();

        }

        private void FormCurve_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.bitmaplayer != null)
            {
                this.bitmaplayer.Dispose();
                this.bitmaplayer = null;
            }

            if (this.curve_pen != null)
            {
                this.curve_pen.Dispose();
                this.curve_pen = null;
            }

            if (this.frame_pen != null)
            {
                this.frame_pen.Dispose();
                this.frame_pen = null;
            }


            if (this.brush != null)
            {
                this.brush.Dispose();
                this.brush = null;
            }
        }

        private void trackBar_Amount_Scroll(object sender, System.EventArgs e)
        {
            update_bend_amount_label();
            this.render_curve();
        }

        private void update_bend_amount_label()
        {
            double v = get_bend_amount_from_trackbar();

            this.labelAmount.Text = v.ToString();
            this.curve.CurveAmount = v;
        }

        private double get_bend_amount_from_trackbar()
        {
            var range = new Numerics.OrderedRangeD(-1, 1);
            double v = this.trackBar_Amount.Value / (double)100;
            v = range.Clamp(v);
            return v;
        }

        private void button_OK_Click(object sender, System.EventArgs e)
        {
            double v = get_smoothing_from_trackbar(this.trackBar_PositionSmoothing);
            this.PositionSmoothingValue = v;

            double v2 = get_smoothing_from_trackbar(this.trackBar_PressureSmoothing);
            this.PressureSmoothingValue = v2;

            state_position_smoothing_trackbar_value = this.trackBar_PositionSmoothing.Value;
            state_pressure_smoothing_trackbar_value = this.trackBar_PressureSmoothing.Value;

            this.Close();
            this.DialogResult = DialogResult.OK;
        }



        private void trackBar_PositionSmoothing_Scroll(object sender, System.EventArgs e)
        {
            var trackbar = this.trackBar_PositionSmoothing;
            var label = label_position_smoothingval;

            update_smoothing_ui(trackbar, label, out PositionSmoothingValue);

        }

        private void trackBar_PressureSmoothing_Scroll(object sender, System.EventArgs e)
        {
            var trackbar = this.trackBar_PressureSmoothing;
            var label = label_pressure_smoothingval;

            update_smoothing_ui(trackbar, label, out this.PressureSmoothingValue);

        }

        private void update_smoothing_ui(System.Windows.Forms.TrackBar trackbar, Label label, out double new_value)
        {
            new_value = get_smoothing_from_trackbar(trackbar);
            string display_val = string.Format("{0:0.0###}", new_value);
            label.Text = display_val;
        }

        private double get_smoothing_from_trackbar(System.Windows.Forms.TrackBar trackbar)
        {
            double normalized_slider_value = ((double)trackbar.Value / (double)trackbar.Maximum);
            double curved_value = this.smoothing_adjustment_curve.ApplyCurve(normalized_slider_value);
            double new_value = PaintSettings.SYS_SMOOTHING_RANGE_LIMITED.Clamp(curved_value);
            return new_value;
        }

        public int PressureQuantizeLevels;

        private void comboBox_PressureQuant_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var q = (QuantItem)comboBox_PressureQuant.SelectedItem;
            this.PressureQuantizeLevels = q.Value;
        }

        private void checkBoxAntiAliasing_CheckedChanged(object sender, System.EventArgs e)
        {
            this.AntiAliasing = checkBoxAntiAliasing.Checked;
        }
    }
}
