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
using WinTabPainter.Numerics;

namespace WinTabPainter
{
    public partial class FormCurve : Form
    {
        public FormCurve(double amt)
        {
            InitializeComponent();
            this.curve = new SimpleCurve();
            this.curve.BendAmount = amt;
        }

        BitmapLayer bitmaplayer;
        SD.Pen pen;
        SD.PointF[] points;
        SimpleCurve curve;
        SD.SolidBrush brush;
        int num_points = 300;

        public double CurveAmount
        {
            get => this.curve.BendAmount;
        } 

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCurve_Load(object sender, EventArgs e)
        {
            var s = new Geometry.Size(num_points, num_points);
            this.bitmaplayer = new BitmapLayer(s);
            this.pictureBox_Curve.Image = this.bitmaplayer.Bitmap;
            this.brush = new SD.SolidBrush(SD.Color.White);
            this.points = new SD.PointF[num_points];
            this.labelAmount.Text = this.curve.BendAmount.ToString();
            this.pen = new SD.Pen(SD.Color.CornflowerBlue,5);

            this.render_curve();

            var curve_slide_range = new Numerics.ValueRangeDouble(-100.0, 100.0);
            var slider_value = (int)HelperMethods.ClampRangeDouble(this.curve.BendAmount * 100.0, curve_slide_range);
            this.trackBar_Amount.Value = slider_value;


        }

        private void render_curve()
        {
            int i_max = num_points - 1;


            var x_coord_range = new Numerics.ValueRangeDouble(0, i_max);
            var y_coord_range = new Numerics.ValueRangeDouble(0, i_max);

            for (int i = 0; i <= i_max; i++)
            {
                double x = i / (double)i_max;
                double y = curve.ApplyCurve(x);

                double x_coord = HelperMethods.ClampRangeDouble(x * i_max, x_coord_range);
                double y_coord = i_max - HelperMethods.ClampRangeDouble(y * i_max, y_coord_range);

                var p = new SD.PointF( (float) x_coord, (float) y_coord);
                this.points[i] = p;



            }

            this.bitmaplayer.Graphics.FillRectangle(this.brush, new SD.Rectangle(0, 0, this.bitmaplayer.Width, this.bitmaplayer.Height));
            this.bitmaplayer.Graphics.DrawLines(this.pen, this.points);
            this.pictureBox_Curve.Invalidate();

        }

        private void FormCurve_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.bitmaplayer != null)
            {
                this.bitmaplayer.Dispose();
                this.bitmaplayer = null;
            }

            if (this.pen != null)
            {
                this.pen.Dispose();
                this.pen = null;
            }

            if (this.brush!=null)
            {
                this.brush.Dispose();  
                this.brush = null;
            }
        }

        private void trackBar_Amount_Scroll(object sender, EventArgs e)
        {
            update_bend_amount_label();
            this.render_curve();
        }

        private void update_bend_amount_label()
        {
            double v = get_bend_amount_from_trackbar();

            this.labelAmount.Text = v.ToString();
            this.curve.BendAmount= v;
        }

        private double get_bend_amount_from_trackbar()
        {
            var range = new Numerics.ValueRangeDouble(-1, 1);
            double v = this.trackBar_Amount.Value / (double)100;
            v = HelperMethods.ClampRangeDouble(v, range);
            return v;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {

            this.Close();
            this.DialogResult = DialogResult.OK;
        }
    }
}
