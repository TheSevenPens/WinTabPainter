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
            label_pressure_level = new Label();
            label2 = new Label();
            label_or_azimuth = new Label();
            label4 = new Label();
            label_or_altitude = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label_pressure_raw
            // 
            label_pressure_raw.AutoSize = true;
            label_pressure_raw.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_pressure_raw.Location = new Point(62, 64);
            label_pressure_raw.Name = "label_pressure_raw";
            label_pressure_raw.Size = new Size(283, 102);
            label_pressure_raw.TabIndex = 0;
            label_pressure_raw.Text = "-----";
            // 
            // label_raw_pressure_txt
            // 
            label_raw_pressure_txt.AutoSize = true;
            label_raw_pressure_txt.Location = new Point(53, 23);
            label_raw_pressure_txt.Name = "label_raw_pressure_txt";
            label_raw_pressure_txt.Size = new Size(193, 41);
            label_raw_pressure_txt.TabIndex = 1;
            label_raw_pressure_txt.Text = "Raw Pressure";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(378, 23);
            label1.Name = "label1";
            label1.Size = new Size(205, 41);
            label1.TabIndex = 3;
            label1.Text = "Pressure Level";
            // 
            // label_pressure_level
            // 
            label_pressure_level.AutoSize = true;
            label_pressure_level.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_pressure_level.Location = new Point(387, 64);
            label_pressure_level.Name = "label_pressure_level";
            label_pressure_level.Size = new Size(379, 102);
            label_pressure_level.TabIndex = 2;
            label_pressure_level.Text = "-------";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(448, 277);
            label2.Name = "label2";
            label2.Size = new Size(282, 41);
            label2.TabIndex = 7;
            label2.Text = "Orientation azimuth";
            // 
            // label_or_azimuth
            // 
            label_or_azimuth.AutoSize = true;
            label_or_azimuth.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_or_azimuth.Location = new Point(457, 318);
            label_or_azimuth.Name = "label_or_azimuth";
            label_or_azimuth.Size = new Size(187, 102);
            label_or_azimuth.TabIndex = 6;
            label_or_azimuth.Text = "---";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(62, 277);
            label4.Name = "label4";
            label4.Size = new Size(276, 41);
            label4.TabIndex = 5;
            label4.Text = "Orientation altitude";
            // 
            // label_or_altitude
            // 
            label_or_altitude.AutoSize = true;
            label_or_altitude.Font = new Font("Consolas", 26.1F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_or_altitude.Location = new Point(71, 318);
            label_or_altitude.Name = "label_or_altitude";
            label_or_altitude.Size = new Size(187, 102);
            label_or_altitude.TabIndex = 4;
            label_or_altitude.Text = "---";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(1098, 64);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(600, 500);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // FormPressureTester
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2023, 1388);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label_or_azimuth);
            Controls.Add(label4);
            Controls.Add(label_or_altitude);
            Controls.Add(label1);
            Controls.Add(label_pressure_level);
            Controls.Add(label_raw_pressure_txt);
            Controls.Add(label_pressure_raw);
            Name = "FormPressureTester";
            Text = "WinTabPressureTester";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_pressure_raw;
        private Label label_raw_pressure_txt;
        private Label label1;
        private Label label_pressure_level;
        private Label label2;
        private Label label_or_azimuth;
        private Label label4;
        private Label label_or_altitude;
        private PictureBox pictureBox1;
    }
}
