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
            label_recordcount = new Label();
            button_clearlog = new Button();
            label7 = new Label();
            label8 = new Label();
            button_clearlast = new Button();
            SuspendLayout();
            // 
            // label_pressure_raw
            // 
            label_pressure_raw.AutoSize = true;
            label_pressure_raw.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_pressure_raw.Location = new Point(326, 18);
            label_pressure_raw.Margin = new Padding(2, 0, 2, 0);
            label_pressure_raw.Name = "label_pressure_raw";
            label_pressure_raw.Size = new Size(154, 56);
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
            label1.Location = new Point(45, 108);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(185, 32);
            label1.TabIndex = 3;
            label1.Text = "Logical pressure";
            // 
            // label_normalized_pressure
            // 
            label_normalized_pressure.AutoSize = true;
            label_normalized_pressure.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_normalized_pressure.Location = new Point(324, 108);
            label_normalized_pressure.Margin = new Padding(2, 0, 2, 0);
            label_normalized_pressure.Name = "label_normalized_pressure";
            label_normalized_pressure.Size = new Size(206, 56);
            label_normalized_pressure.TabIndex = 2;
            label_normalized_pressure.Text = "-------";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 247);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(228, 32);
            label2.TabIndex = 7;
            label2.Text = "Orientation azimuth";
            // 
            // label_or_azimuth
            // 
            label_or_azimuth.AutoSize = true;
            label_or_azimuth.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_or_azimuth.Location = new Point(333, 247);
            label_or_azimuth.Margin = new Padding(2, 0, 2, 0);
            label_or_azimuth.Name = "label_or_azimuth";
            label_or_azimuth.Size = new Size(102, 56);
            label_or_azimuth.TabIndex = 6;
            label_or_azimuth.Text = "---";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(48, 176);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(223, 32);
            label4.TabIndex = 5;
            label4.Text = "Orientation altitude";
            // 
            // label_or_altitude
            // 
            label_or_altitude.AutoSize = true;
            label_or_altitude.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_or_altitude.Location = new Point(333, 176);
            label_or_altitude.Margin = new Padding(2, 0, 2, 0);
            label_or_altitude.Name = "label_or_altitude";
            label_or_altitude.Size = new Size(102, 56);
            label_or_altitude.TabIndex = 4;
            label_or_altitude.Text = "---";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(54, 403);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(189, 32);
            label3.TabIndex = 12;
            label3.Text = "Orientation tilt y";
            // 
            // label_tilty
            // 
            label_tilty.AutoSize = true;
            label_tilty.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_tilty.Location = new Point(339, 403);
            label_tilty.Margin = new Padding(2, 0, 2, 0);
            label_tilty.Name = "label_tilty";
            label_tilty.Size = new Size(206, 56);
            label_tilty.TabIndex = 11;
            label_tilty.Text = "-------";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(55, 323);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(188, 32);
            label6.TabIndex = 10;
            label6.Text = "Orientation tilt x";
            // 
            // label_tiltx
            // 
            label_tiltx.AutoSize = true;
            label_tiltx.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_tiltx.Location = new Point(333, 323);
            label_tiltx.Margin = new Padding(2, 0, 2, 0);
            label_tiltx.Name = "label_tiltx";
            label_tiltx.Size = new Size(206, 56);
            label_tiltx.TabIndex = 9;
            label_tiltx.Text = "-------";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(698, 41);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(243, 32);
            label5.TabIndex = 14;
            label5.Text = "Logical pressure (MA)";
            // 
            // label_normalizedpressure_ma
            // 
            label_normalizedpressure_ma.AutoSize = true;
            label_normalizedpressure_ma.BackColor = SystemColors.Info;
            label_normalizedpressure_ma.BorderStyle = BorderStyle.FixedSingle;
            label_normalizedpressure_ma.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_normalizedpressure_ma.Location = new Point(698, 73);
            label_normalizedpressure_ma.Margin = new Padding(2, 0, 2, 0);
            label_normalizedpressure_ma.Name = "label_normalizedpressure_ma";
            label_normalizedpressure_ma.Size = new Size(208, 58);
            label_normalizedpressure_ma.TabIndex = 13;
            label_normalizedpressure_ma.Text = "-------";
            // 
            // textBox_log
            // 
            textBox_log.BorderStyle = BorderStyle.FixedSingle;
            textBox_log.Enabled = false;
            textBox_log.Location = new Point(1240, 108);
            textBox_log.Multiline = true;
            textBox_log.Name = "textBox_log";
            textBox_log.ReadOnly = true;
            textBox_log.ScrollBars = ScrollBars.Vertical;
            textBox_log.Size = new Size(684, 475);
            textBox_log.TabIndex = 15;
            textBox_log.TabStop = false;
            textBox_log.TextChanged += textBox_log_TextChanged;
            textBox_log.Enter += textBox_log_Enter;
            // 
            // button_start
            // 
            button_start.Location = new Point(1240, 56);
            button_start.Name = "button_start";
            button_start.Size = new Size(150, 46);
            button_start.TabIndex = 16;
            button_start.Text = "start";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // button_stop
            // 
            button_stop.Location = new Point(1396, 56);
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
            label_force.BackColor = SystemColors.Info;
            label_force.BorderStyle = BorderStyle.FixedSingle;
            label_force.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_force.Location = new Point(968, 82);
            label_force.Margin = new Padding(2, 0, 2, 0);
            label_force.Name = "label_force";
            label_force.Size = new Size(104, 58);
            label_force.TabIndex = 18;
            label_force.Text = "---";
            // 
            // button1
            // 
            button1.Location = new Point(1240, 603);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 19;
            button1.Text = "&record";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(1410, 603);
            button2.Name = "button2";
            button2.Size = new Size(150, 46);
            button2.TabIndex = 20;
            button2.Text = "copy";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label_recordcount
            // 
            label_recordcount.AutoSize = true;
            label_recordcount.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_recordcount.Location = new Point(1441, 675);
            label_recordcount.Margin = new Padding(2, 0, 2, 0);
            label_recordcount.Name = "label_recordcount";
            label_recordcount.Size = new Size(154, 56);
            label_recordcount.TabIndex = 21;
            label_recordcount.Text = "-----";
            // 
            // button_clearlog
            // 
            button_clearlog.Location = new Point(1566, 603);
            button_clearlog.Name = "button_clearlog";
            button_clearlog.Size = new Size(150, 46);
            button_clearlog.TabIndex = 22;
            button_clearlog.Text = "clear";
            button_clearlog.UseVisualStyleBackColor = true;
            button_clearlog.Click += button_clearlog_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1240, 675);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(155, 32);
            label7.TabIndex = 23;
            label7.Text = "Record count";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(968, 50);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(195, 32);
            label8.TabIndex = 24;
            label8.Text = "Physical pressure";
            // 
            // button_clearlast
            // 
            button_clearlast.Location = new Point(1739, 603);
            button_clearlast.Name = "button_clearlast";
            button_clearlast.Size = new Size(150, 46);
            button_clearlast.TabIndex = 25;
            button_clearlast.Text = "clear last";
            button_clearlast.UseVisualStyleBackColor = true;
            button_clearlast.Click += button_clearlast_Click;
            // 
            // FormPressureTester
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2155, 1250);
            Controls.Add(button_clearlast);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(button_clearlog);
            Controls.Add(label_recordcount);
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
        private Label label_recordcount;
        private Button button_clearlog;
        private Label label7;
        private Label label8;
        private Button button_clearlast;
    }
}
