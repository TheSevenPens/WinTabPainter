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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label_DeviceInfo = new System.Windows.Forms.Label();
            this.button_ClearCanvas = new System.Windows.Forms.Button();
            this.pictureBox_Canvas = new System.Windows.Forms.PictureBox();
            this.label_AltitudeValue = new System.Windows.Forms.Label();
            this.label_PosXValue = new System.Windows.Forms.Label();
            this.label_PosYValue = new System.Windows.Forms.Label();
            this.label_PosZValue = new System.Windows.Forms.Label();
            this.label_PressureValue = new System.Windows.Forms.Label();
            this.label_AzimuthValue = new System.Windows.Forms.Label();
            this.label_ButtonsValue = new System.Windows.Forms.Label();
            this.label_DeviceValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 32);
            this.label2.TabIndex = 9;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 32);
            this.label3.TabIndex = 10;
            this.label3.Text = "Z";
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
            this.label_DeviceInfo.Location = new System.Drawing.Point(23, 607);
            this.label_DeviceInfo.Name = "label_DeviceInfo";
            this.label_DeviceInfo.Size = new System.Drawing.Size(101, 32);
            this.label_DeviceInfo.TabIndex = 18;
            this.label_DeviceInfo.Text = "Device";
            // 
            // button_ClearCanvas
            // 
            this.button_ClearCanvas.Location = new System.Drawing.Point(1577, 627);
            this.button_ClearCanvas.Name = "button_ClearCanvas";
            this.button_ClearCanvas.Size = new System.Drawing.Size(220, 55);
            this.button_ClearCanvas.TabIndex = 20;
            this.button_ClearCanvas.Text = "Clear";
            this.button_ClearCanvas.UseVisualStyleBackColor = true;
            this.button_ClearCanvas.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // pictureBox_Canvas
            // 
            this.pictureBox_Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Canvas.Location = new System.Drawing.Point(461, 27);
            this.pictureBox_Canvas.Name = "pictureBox_Canvas";
            this.pictureBox_Canvas.Size = new System.Drawing.Size(1336, 569);
            this.pictureBox_Canvas.TabIndex = 21;
            this.pictureBox_Canvas.TabStop = false;
            // 
            // label_AltitudeValue
            // 
            this.label_AltitudeValue.AutoSize = true;
            this.label_AltitudeValue.Location = new System.Drawing.Point(192, 270);
            this.label_AltitudeValue.Name = "label_AltitudeValue";
            this.label_AltitudeValue.Size = new System.Drawing.Size(41, 32);
            this.label_AltitudeValue.TabIndex = 26;
            this.label_AltitudeValue.Text = "---";
            // 
            // label_PosXValue
            // 
            this.label_PosXValue.AutoSize = true;
            this.label_PosXValue.Location = new System.Drawing.Point(190, 27);
            this.label_PosXValue.Name = "label_PosXValue";
            this.label_PosXValue.Size = new System.Drawing.Size(41, 32);
            this.label_PosXValue.TabIndex = 22;
            this.label_PosXValue.Text = "---";
            // 
            // label_PosYValue
            // 
            this.label_PosYValue.AutoSize = true;
            this.label_PosYValue.Location = new System.Drawing.Point(190, 71);
            this.label_PosYValue.Name = "label_PosYValue";
            this.label_PosYValue.Size = new System.Drawing.Size(41, 32);
            this.label_PosYValue.TabIndex = 23;
            this.label_PosYValue.Text = "---";
            // 
            // label_PosZValue
            // 
            this.label_PosZValue.AutoSize = true;
            this.label_PosZValue.Location = new System.Drawing.Point(192, 115);
            this.label_PosZValue.Name = "label_PosZValue";
            this.label_PosZValue.Size = new System.Drawing.Size(41, 32);
            this.label_PosZValue.TabIndex = 24;
            this.label_PosZValue.Text = "---";
            // 
            // label_PressureValue
            // 
            this.label_PressureValue.AutoSize = true;
            this.label_PressureValue.Location = new System.Drawing.Point(190, 189);
            this.label_PressureValue.Name = "label_PressureValue";
            this.label_PressureValue.Size = new System.Drawing.Size(41, 32);
            this.label_PressureValue.TabIndex = 25;
            this.label_PressureValue.Text = "---";
            // 
            // label_AzimuthValue
            // 
            this.label_AzimuthValue.AutoSize = true;
            this.label_AzimuthValue.Location = new System.Drawing.Point(190, 314);
            this.label_AzimuthValue.Name = "label_AzimuthValue";
            this.label_AzimuthValue.Size = new System.Drawing.Size(41, 32);
            this.label_AzimuthValue.TabIndex = 27;
            this.label_AzimuthValue.Text = "---";
            // 
            // label_ButtonsValue
            // 
            this.label_ButtonsValue.AutoSize = true;
            this.label_ButtonsValue.Location = new System.Drawing.Point(190, 392);
            this.label_ButtonsValue.Name = "label_ButtonsValue";
            this.label_ButtonsValue.Size = new System.Drawing.Size(41, 32);
            this.label_ButtonsValue.TabIndex = 28;
            this.label_ButtonsValue.Text = "---";
            // 
            // label_DeviceValue
            // 
            this.label_DeviceValue.AutoSize = true;
            this.label_DeviceValue.Location = new System.Drawing.Point(192, 607);
            this.label_DeviceValue.Name = "label_DeviceValue";
            this.label_DeviceValue.Size = new System.Drawing.Size(41, 32);
            this.label_DeviceValue.TabIndex = 29;
            this.label_DeviceValue.Text = "---";
            // 
            // FormApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1870, 694);
            this.Controls.Add(this.label_DeviceValue);
            this.Controls.Add(this.label_AltitudeValue);
            this.Controls.Add(this.label_PosXValue);
            this.Controls.Add(this.label_PosYValue);
            this.Controls.Add(this.label_PosZValue);
            this.Controls.Add(this.label_PressureValue);
            this.Controls.Add(this.label_AzimuthValue);
            this.Controls.Add(this.label_ButtonsValue);
            this.Controls.Add(this.pictureBox_Canvas);
            this.Controls.Add(this.button_ClearCanvas);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label_DeviceInfo);
            this.Controls.Add(this.label8);
            this.Name = "FormApp";
            this.Text = "WinTab Demo App";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_DeviceInfo;
        private System.Windows.Forms.Button button_ClearCanvas;
        private System.Windows.Forms.PictureBox pictureBox_Canvas;
        private System.Windows.Forms.Label label_AltitudeValue;
        private System.Windows.Forms.Label label_PosXValue;
        private System.Windows.Forms.Label label_PosYValue;
        private System.Windows.Forms.Label label_PosZValue;
        private System.Windows.Forms.Label label_PressureValue;
        private System.Windows.Forms.Label label_AzimuthValue;
        private System.Windows.Forms.Label label_ButtonsValue;
        private System.Windows.Forms.Label label_DeviceValue;
    }
}

