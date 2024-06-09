namespace DemoWinTabPaint1
{
    partial class FormApp
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
            this.textBox_PositionX = new System.Windows.Forms.TextBox();
            this.textBox_PositionY = new System.Windows.Forms.TextBox();
            this.textBox_PositionZ = new System.Windows.Forms.TextBox();
            this.textBox_PressureNormal = new System.Windows.Forms.TextBox();
            this.textBox_OrientationAzimuth = new System.Windows.Forms.TextBox();
            this.textBox_OrientationAltitude = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_Buttons = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label_DeviceInfo = new System.Windows.Forms.Label();
            this.textBox_Device = new System.Windows.Forms.TextBox();
            this.panel_Canvas = new System.Windows.Forms.Panel();
            this.button_Clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_PositionX
            // 
            this.textBox_PositionX.Enabled = false;
            this.textBox_PositionX.Location = new System.Drawing.Point(170, 24);
            this.textBox_PositionX.Name = "textBox_PositionX";
            this.textBox_PositionX.ReadOnly = true;
            this.textBox_PositionX.Size = new System.Drawing.Size(196, 38);
            this.textBox_PositionX.TabIndex = 1;
            // 
            // textBox_PositionY
            // 
            this.textBox_PositionY.Enabled = false;
            this.textBox_PositionY.Location = new System.Drawing.Point(170, 68);
            this.textBox_PositionY.Name = "textBox_PositionY";
            this.textBox_PositionY.ReadOnly = true;
            this.textBox_PositionY.Size = new System.Drawing.Size(196, 38);
            this.textBox_PositionY.TabIndex = 2;
            // 
            // textBox_PositionZ
            // 
            this.textBox_PositionZ.Enabled = false;
            this.textBox_PositionZ.Location = new System.Drawing.Point(170, 112);
            this.textBox_PositionZ.Name = "textBox_PositionZ";
            this.textBox_PositionZ.ReadOnly = true;
            this.textBox_PositionZ.Size = new System.Drawing.Size(196, 38);
            this.textBox_PositionZ.TabIndex = 3;
            // 
            // textBox_PressureNormal
            // 
            this.textBox_PressureNormal.Enabled = false;
            this.textBox_PressureNormal.Location = new System.Drawing.Point(166, 186);
            this.textBox_PressureNormal.Name = "textBox_PressureNormal";
            this.textBox_PressureNormal.ReadOnly = true;
            this.textBox_PressureNormal.Size = new System.Drawing.Size(196, 38);
            this.textBox_PressureNormal.TabIndex = 4;
            // 
            // textBox_OrientationAzimuth
            // 
            this.textBox_OrientationAzimuth.Enabled = false;
            this.textBox_OrientationAzimuth.Location = new System.Drawing.Point(172, 308);
            this.textBox_OrientationAzimuth.Name = "textBox_OrientationAzimuth";
            this.textBox_OrientationAzimuth.ReadOnly = true;
            this.textBox_OrientationAzimuth.Size = new System.Drawing.Size(196, 38);
            this.textBox_OrientationAzimuth.TabIndex = 7;
            // 
            // textBox_OrientationAltitude
            // 
            this.textBox_OrientationAltitude.Enabled = false;
            this.textBox_OrientationAltitude.Location = new System.Drawing.Point(172, 264);
            this.textBox_OrientationAltitude.Name = "textBox_OrientationAltitude";
            this.textBox_OrientationAltitude.ReadOnly = true;
            this.textBox_OrientationAltitude.Size = new System.Drawing.Size(196, 38);
            this.textBox_OrientationAltitude.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Pos. X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 32);
            this.label2.TabIndex = 9;
            this.label2.Text = "Pos. Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 32);
            this.label3.TabIndex = 10;
            this.label3.Text = "Pos. Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 32);
            this.label4.TabIndex = 11;
            this.label4.Text = "Pressure";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 314);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 32);
            this.label6.TabIndex = 14;
            this.label6.Text = "Azimuth";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 32);
            this.label7.TabIndex = 13;
            this.label7.Text = "Altitude";
            // 
            // textBox_Buttons
            // 
            this.textBox_Buttons.Enabled = false;
            this.textBox_Buttons.Location = new System.Drawing.Point(172, 386);
            this.textBox_Buttons.Name = "textBox_Buttons";
            this.textBox_Buttons.ReadOnly = true;
            this.textBox_Buttons.Size = new System.Drawing.Size(196, 38);
            this.textBox_Buttons.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 392);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 32);
            this.label8.TabIndex = 16;
            this.label8.Text = "Buttons";
            // 
            // label_DeviceInfo
            // 
            this.label_DeviceInfo.AutoSize = true;
            this.label_DeviceInfo.Location = new System.Drawing.Point(39, 628);
            this.label_DeviceInfo.Name = "label_DeviceInfo";
            this.label_DeviceInfo.Size = new System.Drawing.Size(101, 32);
            this.label_DeviceInfo.TabIndex = 18;
            this.label_DeviceInfo.Text = "Device";
            // 
            // textBox_Device
            // 
            this.textBox_Device.Enabled = false;
            this.textBox_Device.Location = new System.Drawing.Point(172, 622);
            this.textBox_Device.Name = "textBox_Device";
            this.textBox_Device.ReadOnly = true;
            this.textBox_Device.Size = new System.Drawing.Size(400, 38);
            this.textBox_Device.TabIndex = 15;
            // 
            // panel_Canvas
            // 
            this.panel_Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Canvas.Location = new System.Drawing.Point(426, 50);
            this.panel_Canvas.Name = "panel_Canvas";
            this.panel_Canvas.Size = new System.Drawing.Size(1399, 580);
            this.panel_Canvas.TabIndex = 19;
            this.panel_Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Canvas_Paint);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(29, 495);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(220, 55);
            this.button_Clear.TabIndex = 20;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // FormApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1870, 694);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.panel_Canvas);
            this.Controls.Add(this.textBox_PositionX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_PositionY);
            this.Controls.Add(this.textBox_PositionZ);
            this.Controls.Add(this.textBox_OrientationAltitude);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_PressureNormal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_OrientationAzimuth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_Device);
            this.Controls.Add(this.label_DeviceInfo);
            this.Controls.Add(this.textBox_Buttons);
            this.Controls.Add(this.label8);
            this.Name = "FormApp";
            this.Text = "WinTab Demo App";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_PositionX;
        private System.Windows.Forms.TextBox textBox_PositionY;
        private System.Windows.Forms.TextBox textBox_PositionZ;
        private System.Windows.Forms.TextBox textBox_PressureNormal;
        private System.Windows.Forms.TextBox textBox_OrientationAzimuth;
        private System.Windows.Forms.TextBox textBox_OrientationAltitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_Buttons;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_DeviceInfo;
        private System.Windows.Forms.TextBox textBox_Device;
        private System.Windows.Forms.Panel panel_Canvas;
        private System.Windows.Forms.Button button_Clear;
    }
}

