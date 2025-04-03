namespace WinTabPressureTester
{
    partial class FormPressureTester
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label_pressure_raw = new Label();
            label_raw_pressure_txt = new Label();
            label1 = new Label();
            label_normalized_pressure = new Label();
            label2 = new Label();
            label_or_azimuth = new Label();
            label4 = new Label();
            label_or_altitude = new Label();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            label_tilty = new Label();
            label6 = new Label();
            label_tiltx = new Label();
            label5 = new Label();
            label_normalizedpressure_ma = new Label();
            textBox_log = new TextBox();
            button_start = new Button();
            button_stop = new Button();
            label_force = new Label();
            button1 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label_pressure_raw
            // 
            label_pressure_raw.AutoSize = true;
            label_pressure_raw.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_pressure_raw.Location = new Point(326, 18);
            label_pressure_raw.Margin = new Padding(2, 0, 2, 0);
            label_pressure_raw.Name = "label_pressure_raw";
            label_pressure_raw.Size = new Size(225, 82);
            label_pressure_raw.TabIndex = 0;
            label_pressure_raw.Text = "-----";
            // 
            // label_raw_pressure_txt
            // 
            label_raw_pressure_txt.AutoSize = true;
            label_raw_pressure_txt.Location = new Point(41, 50);
            label_raw_pressure_txt.Margin = new Padding(2, 0, 2, 0);
            label_raw_pressure_txt.Name = "label_raw_pressure_txt";
            label_raw_pressure_txt.Size = new Size(153, 32);
            label_raw_pressure_txt.TabIndex = 1;
            label_raw_pressure_txt.Text = "Raw Pressure";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(47, 151);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(229, 32);
            label1.TabIndex = 3;
            label1.Text = "Pressure normalized";
            // 
            // label_normalized_pressure
            // 
            label_normalized_pressure.AutoSize = true;
            label_normalized_pressure.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_normalized_pressure.Location = new Point(326, 151);
            label_normalized_pressure.Margin = new Padding(2, 0, 2, 0);
            label_normalized_pressure.Name = "label_normalized_pressure";
            label_normalized_pressure.Size = new Size(301, 82);
            label_normalized_pressure.TabIndex = 2;
            label_normalized_pressure.Text = "-------";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 552);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(228, 32);
            label2.TabIndex = 7;
            label2.Text = "Orientation azimuth";
            // 
            // label_or_azimuth
            // 
            label_or_azimuth.AutoSize = true;
            label_or_azimuth.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_or_azimuth.Location = new Point(326, 552);
            label_or_azimuth.Margin = new Padding(2, 0, 2, 0);
            label_or_azimuth.Name = "label_or_azimuth";
            label_or_azimuth.Size = new Size(149, 82);
            label_or_azimuth.TabIndex = 6;
            label_or_azimuth.Text = "---";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 418);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(223, 32);
            label4.TabIndex = 5;
            label4.Text = "Orientation altitude";
            // 
            // label_or_altitude
            // 
            label_or_altitude.AutoSize = true;
            label_or_altitude.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_or_altitude.Location = new Point(326, 418);
            label_or_altitude.Margin = new Padding(2, 0, 2, 0);
            label_or_altitude.Name = "label_or_altitude";
            label_or_altitude.Size = new Size(149, 82);
            label_or_altitude.TabIndex = 4;
            label_or_altitude.Text = "---";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(840, 50);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(459, 390);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 819);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(189, 32);
            label3.TabIndex = 12;
            label3.Text = "Orientation tilt y";
            // 
            // label_tilty
            // 
            label_tilty.AutoSize = true;
            label_tilty.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_tilty.Location = new Point(326, 819);
            label_tilty.Margin = new Padding(2, 0, 2, 0);
            label_tilty.Name = "label_tilty";
            label_tilty.Size = new Size(301, 82);
            label_tilty.TabIndex = 11;
            label_tilty.Text = "-------";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(48, 685);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(188, 32);
            label6.TabIndex = 10;
            label6.Text = "Orientation tilt x";
            // 
            // label_tiltx
            // 
            label_tiltx.AutoSize = true;
            label_tiltx.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_tiltx.Location = new Point(326, 685);
            label_tiltx.Margin = new Padding(2, 0, 2, 0);
            label_tiltx.Name = "label_tiltx";
            label_tiltx.Size = new Size(301, 82);
            label_tiltx.TabIndex = 9;
            label_tiltx.Text = "-------";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(41, 285);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(287, 32);
            label5.TabIndex = 14;
            label5.Text = "Pressure normalized (MA)";
            // 
            // label_normalizedpressure_ma
            // 
            label_normalizedpressure_ma.AutoSize = true;
            label_normalizedpressure_ma.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_normalizedpressure_ma.Location = new Point(326, 285);
            label_normalizedpressure_ma.Margin = new Padding(2, 0, 2, 0);
            label_normalizedpressure_ma.Name = "label_normalizedpressure_ma";
            label_normalizedpressure_ma.Size = new Size(301, 82);
            label_normalizedpressure_ma.TabIndex = 13;
            label_normalizedpressure_ma.Text = "-------";
            // 
            // textBox_log
            // 
            textBox_log.Location = new Point(1435, 99);
            textBox_log.Multiline = true;
            textBox_log.Name = "textBox_log";
            textBox_log.Size = new Size(684, 902);
            textBox_log.TabIndex = 15;
            // 
            // button_start
            // 
            button_start.Location = new Point(908, 576);
            button_start.Name = "button_start";
            button_start.Size = new Size(150, 46);
            button_start.TabIndex = 16;
            button_start.Text = "start";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // button_stop
            // 
            button_stop.Location = new Point(908, 671);
            button_stop.Name = "button_stop";
            button_stop.Size = new Size(150, 46);
            button_stop.TabIndex = 17;
            button_stop.Text = "stop";
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button_stop_Click;
            // 
            // label_force
            // 
            label_force.AutoSize = true;
            label_force.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_force.Location = new Point(908, 462);
            label_force.Margin = new Padding(2, 0, 2, 0);
            label_force.Name = "label_force";
            label_force.Size = new Size(149, 82);
            label_force.TabIndex = 18;
            label_force.Text = "---";
            // 
            // button1
            // 
            button1.Location = new Point(908, 762);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 19;
            button1.Text = "&record";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(908, 851);
            button2.Name = "button2";
            button2.Size = new Size(150, 46);
            button2.TabIndex = 20;
            button2.Text = "copy";
            button2.UseVisualStyleBackColor = true;
            // 
            // FormPressureTester
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2201, 1063);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label_force);
            Controls.Add(button_stop);
            Controls.Add(button_start);
            Controls.Add(textBox_log);
            Controls.Add(label5);
            Controls.Add(label_normalizedpressure_ma);
            Controls.Add(label3);
            Controls.Add(label_tilty);
            Controls.Add(label6);
            Controls.Add(label_tiltx);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label_or_azimuth);
            Controls.Add(label4);
            Controls.Add(label_or_altitude);
            Controls.Add(label1);
            Controls.Add(label_normalized_pressure);
            Controls.Add(label_raw_pressure_txt);
            Controls.Add(label_pressure_raw);
            Margin = new Padding(2);
            Name = "FormPressureTester";
            Text = "WinTabPressureTester";
            FormClosing += FormPressureTester_FormClosing;
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            KeyDown += FormPressureTester_KeyDown;
            KeyPress += FormPressureTester_KeyPress;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_pressure_raw;
        private Label label_raw_pressure_txt;
        private Label label1;
        private Label label_normalized_pressure;
        private Label label2;
        private Label label_or_azimuth;
        private Label label4;
        private Label label_or_altitude;
        private PictureBox pictureBox1;
        private Label label3;
        private Label label_tilty;
        private Label label6;
        private Label label_tiltx;
        private Label label5;
        private Label label_normalizedpressure_ma;
        private TextBox textBox_log;
        private Button button_start;
        private Button button_stop;
        private Label label_force;
        private Button button1;
        private Button button2;
    }
}
