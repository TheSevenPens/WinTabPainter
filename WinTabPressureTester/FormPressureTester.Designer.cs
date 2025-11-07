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
            button_record = new Button();
            button_copytext = new Button();
            label_recordcount = new Label();
            button_clearlog = new Button();
            label7 = new Label();
            label8 = new Label();
            button_clearlast = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            button_load_sample_data = new Button();
            label9 = new Label();
            textBox_brand = new TextBox();
            textBox_Pen = new TextBox();
            label10 = new Label();
            textBox_inventoryid = new TextBox();
            label11 = new Label();
            textBox_date = new TextBox();
            label12 = new Label();
            textBox_User = new TextBox();
            label13 = new Label();
            textBox_Tablet = new TextBox();
            label14 = new Label();
            textBox_driver = new TextBox();
            label15 = new Label();
            textBox_OS = new TextBox();
            label16 = new Label();
            button_export = new Button();
            button_update_chart_title = new Button();
            button_copy_chart = new Button();
            comboBoxcomport = new ComboBox();
            checkBox_tipdown = new CheckBox();
            checkBox_lowerbuttondown = new CheckBox();
            checkBox_upperbuttondown = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label_pressure_raw
            // 
            label_pressure_raw.AutoSize = true;
            label_pressure_raw.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_pressure_raw.Location = new Point(254, 42);
            label_pressure_raw.Margin = new Padding(2, 0, 2, 0);
            label_pressure_raw.Name = "label_pressure_raw";
            label_pressure_raw.Size = new Size(70, 24);
            label_pressure_raw.TabIndex = 0;
            label_pressure_raw.Text = "-----";
            // 
            // label_raw_pressure_txt
            // 
            label_raw_pressure_txt.AutoSize = true;
            label_raw_pressure_txt.Location = new Point(14, 38);
            label_raw_pressure_txt.Margin = new Padding(2, 0, 2, 0);
            label_raw_pressure_txt.Name = "label_raw_pressure_txt";
            label_raw_pressure_txt.Size = new Size(136, 30);
            label_raw_pressure_txt.TabIndex = 1;
            label_raw_pressure_txt.Text = "Raw Pressure";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 78);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(162, 30);
            label1.TabIndex = 3;
            label1.Text = "Logical pressure";
            // 
            // label_normalized_pressure
            // 
            label_normalized_pressure.AutoSize = true;
            label_normalized_pressure.Font = new Font("Consolas", 9F);
            label_normalized_pressure.Location = new Point(254, 80);
            label_normalized_pressure.Margin = new Padding(2, 0, 2, 0);
            label_normalized_pressure.Name = "label_normalized_pressure";
            label_normalized_pressure.Size = new Size(94, 24);
            label_normalized_pressure.TabIndex = 2;
            label_normalized_pressure.Text = "-------";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 155);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(199, 30);
            label2.TabIndex = 7;
            label2.Text = "Orientation azimuth";
            // 
            // label_or_azimuth
            // 
            label_or_azimuth.AutoSize = true;
            label_or_azimuth.Font = new Font("Consolas", 9F);
            label_or_azimuth.Location = new Point(254, 156);
            label_or_azimuth.Margin = new Padding(2, 0, 2, 0);
            label_or_azimuth.Name = "label_or_azimuth";
            label_or_azimuth.Size = new Size(46, 24);
            label_or_azimuth.TabIndex = 6;
            label_or_azimuth.Text = "---";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 116);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(194, 30);
            label4.TabIndex = 5;
            label4.Text = "Orientation altitude";
            // 
            // label_or_altitude
            // 
            label_or_altitude.AutoSize = true;
            label_or_altitude.Font = new Font("Consolas", 9F);
            label_or_altitude.Location = new Point(254, 119);
            label_or_altitude.Margin = new Padding(2, 0, 2, 0);
            label_or_altitude.Name = "label_or_altitude";
            label_or_altitude.Size = new Size(46, 24);
            label_or_altitude.TabIndex = 4;
            label_or_altitude.Text = "---";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(775, 47);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(424, 366);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 233);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(164, 30);
            label3.TabIndex = 12;
            label3.Text = "Orientation tilt y";
            // 
            // label_tilty
            // 
            label_tilty.AutoSize = true;
            label_tilty.Font = new Font("Consolas", 9F);
            label_tilty.Location = new Point(254, 233);
            label_tilty.Margin = new Padding(2, 0, 2, 0);
            label_tilty.Name = "label_tilty";
            label_tilty.Size = new Size(94, 24);
            label_tilty.TabIndex = 11;
            label_tilty.Text = "-------";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 194);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(164, 30);
            label6.TabIndex = 10;
            label6.Text = "Orientation tilt x";
            // 
            // label_tiltx
            // 
            label_tiltx.AutoSize = true;
            label_tiltx.Font = new Font("Consolas", 9F);
            label_tiltx.Location = new Point(254, 194);
            label_tiltx.Margin = new Padding(2, 0, 2, 0);
            label_tiltx.Name = "label_tiltx";
            label_tiltx.Size = new Size(94, 24);
            label_tiltx.TabIndex = 9;
            label_tiltx.Text = "-------";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(395, 20);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(213, 30);
            label5.TabIndex = 14;
            label5.Text = "Logical pressure (MA)";
            // 
            // label_normalizedpressure_ma
            // 
            label_normalizedpressure_ma.BackColor = SystemColors.Info;
            label_normalizedpressure_ma.BorderStyle = BorderStyle.FixedSingle;
            label_normalizedpressure_ma.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_normalizedpressure_ma.Location = new Point(401, 61);
            label_normalizedpressure_ma.Margin = new Padding(2, 0, 2, 0);
            label_normalizedpressure_ma.Name = "label_normalizedpressure_ma";
            label_normalizedpressure_ma.Size = new Size(187, 54);
            label_normalizedpressure_ma.TabIndex = 13;
            label_normalizedpressure_ma.Text = "-------";
            // 
            // textBox_log
            // 
            textBox_log.BackColor = SystemColors.Info;
            textBox_log.BorderStyle = BorderStyle.FixedSingle;
            textBox_log.Enabled = false;
            textBox_log.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox_log.Location = new Point(29, 676);
            textBox_log.Margin = new Padding(2);
            textBox_log.Multiline = true;
            textBox_log.Name = "textBox_log";
            textBox_log.ReadOnly = true;
            textBox_log.ScrollBars = ScrollBars.Vertical;
            textBox_log.Size = new Size(316, 310);
            textBox_log.TabIndex = 15;
            textBox_log.TabStop = false;
            textBox_log.TextChanged += textBox_log_TextChanged;
            textBox_log.Enter += textBox_log_Enter;
            // 
            // button_start
            // 
            button_start.Location = new Point(401, 226);
            button_start.Margin = new Padding(2);
            button_start.Name = "button_start";
            button_start.Size = new Size(138, 43);
            button_start.TabIndex = 16;
            button_start.Text = "start (&O)";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // button_stop
            // 
            button_stop.Location = new Point(545, 226);
            button_stop.Margin = new Padding(2);
            button_stop.Name = "button_stop";
            button_stop.Size = new Size(138, 43);
            button_stop.TabIndex = 17;
            button_stop.Text = "stop (&P)";
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button_stop_Click;
            // 
            // label_force
            // 
            label_force.BackColor = SystemColors.Info;
            label_force.BorderStyle = BorderStyle.FixedSingle;
            label_force.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_force.Location = new Point(401, 170);
            label_force.Margin = new Padding(2, 0, 2, 0);
            label_force.Name = "label_force";
            label_force.Size = new Size(242, 54);
            label_force.TabIndex = 18;
            label_force.Text = "---";
            // 
            // button_record
            // 
            button_record.Location = new Point(401, 292);
            button_record.Margin = new Padding(2);
            button_record.Name = "button_record";
            button_record.Size = new Size(138, 43);
            button_record.TabIndex = 19;
            button_record.Text = "&record (R)";
            button_record.UseVisualStyleBackColor = true;
            button_record.Click += button_record_Click;
            // 
            // button_copytext
            // 
            button_copytext.Location = new Point(401, 628);
            button_copytext.Margin = new Padding(2);
            button_copytext.Name = "button_copytext";
            button_copytext.Size = new Size(157, 43);
            button_copytext.TabIndex = 20;
            button_copytext.Text = "copy text";
            button_copytext.UseVisualStyleBackColor = true;
            button_copytext.Click += button2_Click;
            // 
            // label_recordcount
            // 
            label_recordcount.AutoSize = true;
            label_recordcount.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_recordcount.Location = new Point(214, 1008);
            label_recordcount.Margin = new Padding(2, 0, 2, 0);
            label_recordcount.Name = "label_recordcount";
            label_recordcount.Size = new Size(70, 24);
            label_recordcount.TabIndex = 21;
            label_recordcount.Text = "-----";
            // 
            // button_clearlog
            // 
            button_clearlog.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button_clearlog.ForeColor = Color.IndianRed;
            button_clearlog.Location = new Point(401, 421);
            button_clearlog.Margin = new Padding(2);
            button_clearlog.Name = "button_clearlog";
            button_clearlog.Size = new Size(138, 43);
            button_clearlog.TabIndex = 22;
            button_clearlog.Text = "clear all";
            button_clearlog.UseVisualStyleBackColor = true;
            button_clearlog.Click += button_clearlog_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(29, 1008);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(136, 30);
            label7.TabIndex = 23;
            label7.Text = "Record count";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(401, 131);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(171, 30);
            label8.TabIndex = 24;
            label8.Text = "Physical pressure";
            // 
            // button_clearlast
            // 
            button_clearlast.Location = new Point(401, 359);
            button_clearlast.Margin = new Padding(2);
            button_clearlast.Name = "button_clearlast";
            button_clearlast.Size = new Size(138, 43);
            button_clearlast.TabIndex = 25;
            button_clearlast.Text = "clear &last (L)";
            button_clearlast.UseVisualStyleBackColor = true;
            button_clearlast.Click += button_clearlast_Click;
            // 
            // formsPlot1
            // 
            formsPlot1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot1.DisplayScale = 2F;
            formsPlot1.Location = new Point(709, 20);
            formsPlot1.Margin = new Padding(2);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(929, 839);
            formsPlot1.TabIndex = 26;
            formsPlot1.Load += formsPlot1_Load;
            // 
            // button_load_sample_data
            // 
            button_load_sample_data.Location = new Point(401, 481);
            button_load_sample_data.Margin = new Padding(2);
            button_load_sample_data.Name = "button_load_sample_data";
            button_load_sample_data.Size = new Size(210, 43);
            button_load_sample_data.TabIndex = 27;
            button_load_sample_data.Text = "load sample data";
            button_load_sample_data.UseVisualStyleBackColor = true;
            button_load_sample_data.Click += button_load_sample_data_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(23, 294);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(67, 30);
            label9.TabIndex = 28;
            label9.Text = "Brand";
            // 
            // textBox_brand
            // 
            textBox_brand.BorderStyle = BorderStyle.FixedSingle;
            textBox_brand.Font = new Font("Consolas", 9F);
            textBox_brand.Location = new Point(158, 296);
            textBox_brand.Margin = new Padding(2);
            textBox_brand.Name = "textBox_brand";
            textBox_brand.Size = new Size(177, 32);
            textBox_brand.TabIndex = 29;
            textBox_brand.Text = "BRAND";
            textBox_brand.TextChanged += textBox_brand_TextChanged;
            textBox_brand.KeyDown += textBox_brand_KeyDown;
            textBox_brand.KeyUp += textBox_brand_KeyUp;
            // 
            // textBox_Pen
            // 
            textBox_Pen.BorderStyle = BorderStyle.FixedSingle;
            textBox_Pen.Font = new Font("Consolas", 9F);
            textBox_Pen.Location = new Point(158, 341);
            textBox_Pen.Margin = new Padding(2);
            textBox_Pen.Name = "textBox_Pen";
            textBox_Pen.Size = new Size(177, 32);
            textBox_Pen.TabIndex = 31;
            textBox_Pen.Text = "KP-504E";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(23, 338);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(47, 30);
            label10.TabIndex = 30;
            label10.Text = "Pen";
            // 
            // textBox_inventoryid
            // 
            textBox_inventoryid.BorderStyle = BorderStyle.FixedSingle;
            textBox_inventoryid.Font = new Font("Consolas", 9F);
            textBox_inventoryid.Location = new Point(158, 385);
            textBox_inventoryid.Margin = new Padding(2);
            textBox_inventoryid.Name = "textBox_inventoryid";
            textBox_inventoryid.Size = new Size(177, 32);
            textBox_inventoryid.TabIndex = 33;
            textBox_inventoryid.Text = "--P.0000";
            textBox_inventoryid.TextChanged += textBox_inventoryid_TextChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(23, 383);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(121, 30);
            label11.TabIndex = 32;
            label11.Text = "InventoryID";
            // 
            // textBox_date
            // 
            textBox_date.BorderStyle = BorderStyle.FixedSingle;
            textBox_date.Font = new Font("Consolas", 9F);
            textBox_date.Location = new Point(158, 431);
            textBox_date.Margin = new Padding(2);
            textBox_date.Name = "textBox_date";
            textBox_date.Size = new Size(177, 32);
            textBox_date.TabIndex = 35;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(23, 427);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(57, 30);
            label12.TabIndex = 34;
            label12.Text = "Date";
            // 
            // textBox_User
            // 
            textBox_User.BorderStyle = BorderStyle.FixedSingle;
            textBox_User.Font = new Font("Consolas", 9F);
            textBox_User.Location = new Point(158, 475);
            textBox_User.Margin = new Padding(2);
            textBox_User.Name = "textBox_User";
            textBox_User.Size = new Size(177, 32);
            textBox_User.TabIndex = 37;
            textBox_User.Text = "USERNAME";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(23, 472);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.Size = new Size(54, 30);
            label13.TabIndex = 36;
            label13.Text = "User";
            // 
            // textBox_Tablet
            // 
            textBox_Tablet.BorderStyle = BorderStyle.FixedSingle;
            textBox_Tablet.Font = new Font("Consolas", 9F);
            textBox_Tablet.Location = new Point(158, 520);
            textBox_Tablet.Margin = new Padding(2);
            textBox_Tablet.Name = "textBox_Tablet";
            textBox_Tablet.Size = new Size(177, 32);
            textBox_Tablet.TabIndex = 39;
            textBox_Tablet.Text = "PTH-660";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(23, 516);
            label14.Margin = new Padding(2, 0, 2, 0);
            label14.Name = "label14";
            label14.Size = new Size(68, 30);
            label14.TabIndex = 38;
            label14.Text = "Tablet";
            // 
            // textBox_driver
            // 
            textBox_driver.BorderStyle = BorderStyle.FixedSingle;
            textBox_driver.Font = new Font("Consolas", 9F);
            textBox_driver.Location = new Point(158, 564);
            textBox_driver.Margin = new Padding(2);
            textBox_driver.Name = "textBox_driver";
            textBox_driver.Size = new Size(177, 32);
            textBox_driver.TabIndex = 41;
            textBox_driver.Text = "DRIVER";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(23, 562);
            label15.Margin = new Padding(2, 0, 2, 0);
            label15.Name = "label15";
            label15.Size = new Size(68, 30);
            label15.TabIndex = 40;
            label15.Text = "Driver";
            // 
            // textBox_OS
            // 
            textBox_OS.BorderStyle = BorderStyle.FixedSingle;
            textBox_OS.Font = new Font("Consolas", 9F);
            textBox_OS.Location = new Point(158, 608);
            textBox_OS.Margin = new Padding(2);
            textBox_OS.Name = "textBox_OS";
            textBox_OS.Size = new Size(177, 32);
            textBox_OS.TabIndex = 43;
            textBox_OS.Text = "WINDOWS";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(23, 606);
            label16.Margin = new Padding(2, 0, 2, 0);
            label16.Name = "label16";
            label16.Size = new Size(40, 30);
            label16.TabIndex = 42;
            label16.Text = "OS";
            // 
            // button_export
            // 
            button_export.Location = new Point(551, 359);
            button_export.Margin = new Padding(2);
            button_export.Name = "button_export";
            button_export.Size = new Size(138, 43);
            button_export.TabIndex = 44;
            button_export.Text = "export";
            button_export.UseVisualStyleBackColor = true;
            button_export.Click += button_export_Click;
            // 
            // button_update_chart_title
            // 
            button_update_chart_title.Location = new Point(401, 554);
            button_update_chart_title.Margin = new Padding(2);
            button_update_chart_title.Name = "button_update_chart_title";
            button_update_chart_title.Size = new Size(210, 43);
            button_update_chart_title.TabIndex = 45;
            button_update_chart_title.Text = "Update chart title";
            button_update_chart_title.UseVisualStyleBackColor = true;
            button_update_chart_title.Click += button3_Click;
            // 
            // button_copy_chart
            // 
            button_copy_chart.Location = new Point(401, 696);
            button_copy_chart.Margin = new Padding(2);
            button_copy_chart.Name = "button_copy_chart";
            button_copy_chart.Size = new Size(157, 43);
            button_copy_chart.TabIndex = 46;
            button_copy_chart.Text = "copy chart";
            button_copy_chart.UseVisualStyleBackColor = true;
            button_copy_chart.Click += button4_Click;
            // 
            // comboBoxcomport
            // 
            comboBoxcomport.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxcomport.FormattingEnabled = true;
            comboBoxcomport.Items.AddRange(new object[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6" });
            comboBoxcomport.Location = new Point(401, 812);
            comboBoxcomport.Margin = new Padding(2);
            comboBoxcomport.Name = "comboBoxcomport";
            comboBoxcomport.Size = new Size(214, 38);
            comboBoxcomport.TabIndex = 47;
            // 
            // checkBox_tipdown
            // 
            checkBox_tipdown.AutoSize = true;
            checkBox_tipdown.Location = new Point(413, 972);
            checkBox_tipdown.Margin = new Padding(2);
            checkBox_tipdown.Name = "checkBox_tipdown";
            checkBox_tipdown.Size = new Size(121, 34);
            checkBox_tipdown.TabIndex = 48;
            checkBox_tipdown.Text = "TipDown";
            checkBox_tipdown.UseVisualStyleBackColor = true;
            // 
            // checkBox_lowerbuttondown
            // 
            checkBox_lowerbuttondown.AutoSize = true;
            checkBox_lowerbuttondown.Location = new Point(413, 1009);
            checkBox_lowerbuttondown.Margin = new Padding(2);
            checkBox_lowerbuttondown.Name = "checkBox_lowerbuttondown";
            checkBox_lowerbuttondown.Size = new Size(216, 34);
            checkBox_lowerbuttondown.TabIndex = 49;
            checkBox_lowerbuttondown.Text = "LowerButton Down";
            checkBox_lowerbuttondown.UseVisualStyleBackColor = true;
            // 
            // checkBox_upperbuttondown
            // 
            checkBox_upperbuttondown.AutoSize = true;
            checkBox_upperbuttondown.Location = new Point(412, 1048);
            checkBox_upperbuttondown.Margin = new Padding(2);
            checkBox_upperbuttondown.Name = "checkBox_upperbuttondown";
            checkBox_upperbuttondown.Size = new Size(217, 34);
            checkBox_upperbuttondown.TabIndex = 50;
            checkBox_upperbuttondown.Text = "UpperButton Down";
            checkBox_upperbuttondown.UseVisualStyleBackColor = true;
            // 
            // FormPressureTester
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1656, 1050);
            Controls.Add(checkBox_upperbuttondown);
            Controls.Add(checkBox_lowerbuttondown);
            Controls.Add(checkBox_tipdown);
            Controls.Add(comboBoxcomport);
            Controls.Add(button_copy_chart);
            Controls.Add(button_update_chart_title);
            Controls.Add(button_export);
            Controls.Add(textBox_OS);
            Controls.Add(label16);
            Controls.Add(textBox_driver);
            Controls.Add(label15);
            Controls.Add(textBox_Tablet);
            Controls.Add(label14);
            Controls.Add(textBox_User);
            Controls.Add(label13);
            Controls.Add(textBox_date);
            Controls.Add(label12);
            Controls.Add(textBox_inventoryid);
            Controls.Add(label11);
            Controls.Add(textBox_Pen);
            Controls.Add(label10);
            Controls.Add(textBox_brand);
            Controls.Add(label9);
            Controls.Add(button_load_sample_data);
            Controls.Add(formsPlot1);
            Controls.Add(button_clearlast);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(button_clearlog);
            Controls.Add(label_recordcount);
            Controls.Add(button_copytext);
            Controls.Add(button_record);
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
        private Button button_record;
        private Button button_copytext;
        private Label label_recordcount;
        private Button button_clearlog;
        private Label label7;
        private Label label8;
        private Button button_clearlast;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button button_load_sample_data;
        private Label label9;
        private TextBox textBox_brand;
        private TextBox textBox_Pen;
        private Label label10;
        private TextBox textBox_inventoryid;
        private Label label11;
        private TextBox textBox_date;
        private Label label12;
        private TextBox textBox_User;
        private Label label13;
        private TextBox textBox_Tablet;
        private Label label14;
        private TextBox textBox_driver;
        private Label label15;
        private TextBox textBox_OS;
        private Label label16;
        private Button button_export;
        private Button button_update_chart_title;
        private Button button_copy_chart;
        private ComboBox comboBoxcomport;
        private CheckBox checkBox_tipdown;
        private CheckBox checkBox_lowerbuttondown;
        private CheckBox checkBox_upperbuttondown;
    }
}
