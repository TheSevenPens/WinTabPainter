using SD = System.Drawing;
using System.Windows.Forms;


namespace WinTabPainter
{
    public partial class FormCurve : Form
    {
        public FormCurve(double amt)
        {
            InitializeComponent();
            this.curve = new Numerics.SimpleCurve();
            this.curve.BendAmount = amt;
        }

        Painting.BitmapLayer bitmaplayer;
        SD.Pen pen;
        SD.PointF[] points;
        Numerics.SimpleCurve curve;
        SD.SolidBrush brush;
        int padding = 25;
        int num_points = 300;

        public double CurveAmount
        {
            get => this.curve.BendAmount;
        } 

        private void button_Close_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void FormCurve_Load(object sender, System.EventArgs e)
        {
            var s = new Geometry.Size(num_points + 2 * this.padding, num_points+2*this.padding);
            this.bitmaplayer = new Painting.BitmapLayer(s);
            this.pictureBox_Curve.Image = this.bitmaplayer.Bitmap;
            this.brush = new SD.SolidBrush(SD.Color.White);
            this.points = new SD.PointF[num_points];
            this.labelAmount.Text = this.curve.BendAmount.ToString();
            this.pen = new SD.Pen(SD.Color.CornflowerBlue,5);

            this.render_curve();

            var curve_slide_range = new Numerics.RangeD(-100.0, 100.0);
            var slider_value = (int)curve_slide_range.Clamp(this.curve.BendAmount * 100.0);
            this.trackBar_Amount.Value = slider_value;


        }

        private void render_curve()
        {
            int i_max = num_points - 1;


            var x_coord_range = new Numerics.RangeD(0, i_max);
            var y_coord_range = new Numerics.RangeD(0, i_max);

            for (int i = 0; i <= i_max; i++)
            {
                double x = i / (double)i_max;
                double y = curve.ApplyCurve(x);

                double x_coord = x_coord_range.Clamp(x * i_max );
                double y_coord = i_max - y_coord_range.Clamp(y * i_max);

                var p = new Geometry.PointD(x_coord, y_coord);
                var p2 = p.Add(this.padding, this.padding);
                var p3 = new SD.PointF( (float) p2.X, (float) p2.Y );
                this.points[i] = p3;



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

        private void trackBar_Amount_Scroll(object sender, System.EventArgs e)
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
            var range = new Numerics.RangeD(-1, 1);
            double v = this.trackBar_Amount.Value / (double)100;
            v = range.Clamp(v);
            return v;
        }

        private void button_OK_Click(object sender, System.EventArgs e)
        {

            this.Close();
            this.DialogResult = DialogResult.OK;
        }
    }
}
