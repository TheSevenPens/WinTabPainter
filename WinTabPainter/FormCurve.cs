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
        public FormCurve()
        {
            InitializeComponent();
        }

        BitmapLayer bitmaplayer;
        SD.Pen pen;
        SD.PointF[] points;
        SimpleCurve curve;
        SD.SolidBrush brush;

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCurve_Load(object sender, EventArgs e)
        {
            var s = new Geometry.Size(this.pictureBox_Curve.Width, this.pictureBox_Curve.Height);
            this.bitmaplayer = new BitmapLayer(s);
            this.pictureBox_Curve.Image = this.bitmaplayer.Bitmap;
            this.brush = new SD.SolidBrush(SD.Color.White);
            this.points = new SD.PointF[this.pictureBox_Curve.Width];

            this.curve = new SimpleCurve();
            this.curve.BendAmount= 1;

            this.render_curve();

            var slider_value = (int)HelperMethods.ClampRangeDouble(this.curve.BendAmount * 100.0, -100, 100);
            this.trackBar_Amount.Value = slider_value;


        }

        private void render_curve()
        {
            int i_max = this.bitmaplayer.Width - 1;

            this.pen = new SD.Pen(SD.Color.Black);


            for (int i = 0; i <= i_max; i++)
            {
                double x = i / (double)i_max;
                double y = curve.ApplyCurve(x);

                double x_coord = HelperMethods.ClampRangeDouble(x * i_max, 0, i_max);
                double y_coord = HelperMethods.ClampRangeDouble(y * i_max, 0, i_max);

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
            double v = this.trackBar_Amount.Value / (double)100;
            v = HelperMethods.ClampRangeDouble(v, -1, 1);
            return v;
        }
    }
}
