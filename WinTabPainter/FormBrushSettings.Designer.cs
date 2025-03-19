namespace WinTabPainter
{
    partial class FormBrushSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_Cancel = new System.Windows.Forms.Button();
            pictureBox_Curve = new System.Windows.Forms.PictureBox();
            trackBar_Amount = new System.Windows.Forms.TrackBar();
            labelAmount = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button_OK = new System.Windows.Forms.Button();
            label_pressure_smoothingval = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            trackBar_PressureSmoothing = new System.Windows.Forms.TrackBar();
            comboBox_PressureQuant = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            label_position_smoothingval = new System.Windows.Forms.Label();
            trackBar_PositionSmoothing = new System.Windows.Forms.TrackBar();
            label3 = new System.Windows.Forms.Label();
            checkBoxAntiAliasing = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            textBoxPositionNoise = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Curve).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_Amount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_PressureSmoothing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_PositionSmoothing).BeginInit();
            SuspendLayout();
            // 
            // button_Cancel
            // 
            button_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_Cancel.Location = new System.Drawing.Point(1432, 873);
            button_Cancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new System.Drawing.Size(157, 69);
            button_Cancel.TabIndex = 0;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Close_Click;
            // 
            // pictureBox_Curve
            // 
            pictureBox_Curve.Location = new System.Drawing.Point(26, 36);
            pictureBox_Curve.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBox_Curve.Name = "pictureBox_Curve";
            pictureBox_Curve.Size = new System.Drawing.Size(550, 550);
            pictureBox_Curve.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox_Curve.TabIndex = 1;
            pictureBox_Curve.TabStop = false;
            // 
            // trackBar_Amount
            // 
            trackBar_Amount.AutoSize = false;
            trackBar_Amount.Location = new System.Drawing.Point(29, 642);
            trackBar_Amount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_Amount.Maximum = 100;
            trackBar_Amount.Minimum = -100;
            trackBar_Amount.Name = "trackBar_Amount";
            trackBar_Amount.Size = new System.Drawing.Size(460, 36);
            trackBar_Amount.TabIndex = 2;
            trackBar_Amount.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_Amount.Value = 100;
            trackBar_Amount.Scroll += trackBar_Amount_Scroll;
            // 
            // labelAmount
            // 
            labelAmount.AutoSize = true;
            labelAmount.Location = new System.Drawing.Point(488, 637);
            labelAmount.Name = "labelAmount";
            labelAmount.Size = new System.Drawing.Size(84, 41);
            labelAmount.TabIndex = 3;
            labelAmount.Text = "NNN";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(462, 597);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(97, 41);
            label1.TabIndex = 4;
            label1.Text = "Softer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(34, 597);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(108, 41);
            label2.TabIndex = 5;
            label2.Text = "Harder";
            // 
            // button_OK
            // 
            button_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button_OK.Location = new System.Drawing.Point(1241, 873);
            button_OK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button_OK.Name = "button_OK";
            button_OK.Size = new System.Drawing.Size(157, 69);
            button_OK.TabIndex = 7;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // label_pressure_smoothingval
            // 
            label_pressure_smoothingval.AutoSize = true;
            label_pressure_smoothingval.Location = new System.Drawing.Point(1354, 156);
            label_pressure_smoothingval.Name = "label_pressure_smoothingval";
            label_pressure_smoothingval.Size = new System.Drawing.Size(84, 41);
            label_pressure_smoothingval.TabIndex = 53;
            label_pressure_smoothingval.Text = "NNN";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(617, 156);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(285, 41);
            label4.TabIndex = 52;
            label4.Text = "Pressure Smoothing";
            // 
            // trackBar_PressureSmoothing
            // 
            trackBar_PressureSmoothing.AutoSize = false;
            trackBar_PressureSmoothing.Location = new System.Drawing.Point(902, 158);
            trackBar_PressureSmoothing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_PressureSmoothing.Maximum = 100;
            trackBar_PressureSmoothing.Name = "trackBar_PressureSmoothing";
            trackBar_PressureSmoothing.Size = new System.Drawing.Size(450, 36);
            trackBar_PressureSmoothing.TabIndex = 51;
            trackBar_PressureSmoothing.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_PressureSmoothing.Scroll += trackBar_PressureSmoothing_Scroll;
            // 
            // comboBox_PressureQuant
            // 
            comboBox_PressureQuant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox_PressureQuant.FormattingEnabled = true;
            comboBox_PressureQuant.Location = new System.Drawing.Point(925, 219);
            comboBox_PressureQuant.Name = "comboBox_PressureQuant";
            comboBox_PressureQuant.Size = new System.Drawing.Size(302, 49);
            comboBox_PressureQuant.TabIndex = 55;
            comboBox_PressureQuant.SelectedIndexChanged += comboBox_PressureQuant_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(616, 219);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(303, 41);
            label5.TabIndex = 56;
            label5.Text = "Pressure quantization";
            // 
            // label_position_smoothingval
            // 
            label_position_smoothingval.AutoSize = true;
            label_position_smoothingval.Location = new System.Drawing.Point(1354, 41);
            label_position_smoothingval.Name = "label_position_smoothingval";
            label_position_smoothingval.Size = new System.Drawing.Size(84, 41);
            label_position_smoothingval.TabIndex = 54;
            label_position_smoothingval.Text = "NNN";
            // 
            // trackBar_PositionSmoothing
            // 
            trackBar_PositionSmoothing.AutoSize = false;
            trackBar_PositionSmoothing.Location = new System.Drawing.Point(902, 43);
            trackBar_PositionSmoothing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            trackBar_PositionSmoothing.Maximum = 100;
            trackBar_PositionSmoothing.Name = "trackBar_PositionSmoothing";
            trackBar_PositionSmoothing.Size = new System.Drawing.Size(450, 36);
            trackBar_PositionSmoothing.TabIndex = 49;
            trackBar_PositionSmoothing.TickStyle = System.Windows.Forms.TickStyle.None;
            trackBar_PositionSmoothing.Scroll += trackBar_PositionSmoothing_Scroll;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(617, 41);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(279, 41);
            label3.TabIndex = 50;
            label3.Text = "Position Smoothing";
            // 
            // checkBoxAntiAliasing
            // 
            checkBoxAntiAliasing.AutoSize = true;
            checkBoxAntiAliasing.Location = new System.Drawing.Point(626, 336);
            checkBoxAntiAliasing.Name = "checkBoxAntiAliasing";
            checkBoxAntiAliasing.Size = new System.Drawing.Size(224, 45);
            checkBoxAntiAliasing.TabIndex = 57;
            checkBoxAntiAliasing.Text = "Anti-Aliasing";
            checkBoxAntiAliasing.UseVisualStyleBackColor = true;
            checkBoxAntiAliasing.CheckedChanged += checkBoxAntiAliasing_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(626, 445);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(216, 41);
            label6.TabIndex = 58;
            label6.Text = " Position Noise";
            // 
            // textBoxPositionNoise
            // 
            textBoxPositionNoise.Location = new System.Drawing.Point(869, 453);
            textBoxPositionNoise.Name = "textBoxPositionNoise";
            textBoxPositionNoise.Size = new System.Drawing.Size(250, 47);
            textBoxPositionNoise.TabIndex = 59;
            // 
            // FormBrushSettings
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1601, 965);
            Controls.Add(textBoxPositionNoise);
            Controls.Add(label6);
            Controls.Add(checkBoxAntiAliasing);
            Controls.Add(label5);
            Controls.Add(comboBox_PressureQuant);
            Controls.Add(label_position_smoothingval);
            Controls.Add(label_pressure_smoothingval);
            Controls.Add(label4);
            Controls.Add(trackBar_PressureSmoothing);
            Controls.Add(label3);
            Controls.Add(trackBar_PositionSmoothing);
            Controls.Add(button_OK);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelAmount);
            Controls.Add(trackBar_Amount);
            Controls.Add(pictureBox_Curve);
            Controls.Add(button_Cancel);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "FormBrushSettings";
            Text = "Brush settings";
            FormClosed += FormCurve_FormClosed;
            Load += FormCurve_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_Curve).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_Amount).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_PressureSmoothing).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_PositionSmoothing).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TrackBar trackBar_Amount;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_OK;
        protected System.Windows.Forms.PictureBox pictureBox_Curve;
        private System.Windows.Forms.Label label_pressure_smoothingval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBar_PressureSmoothing;
        private System.Windows.Forms.ComboBox comboBox_PressureQuant;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_position_smoothingval;
        private System.Windows.Forms.TrackBar trackBar_PositionSmoothing;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxAntiAliasing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPositionNoise;
    }
}