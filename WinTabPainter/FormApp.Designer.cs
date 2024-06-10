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
            this.label_X = new System.Windows.Forms.Label();
            this.label_Y = new System.Windows.Forms.Label();
            this.label_Z = new System.Windows.Forms.Label();
            this.label_Pressure = new System.Windows.Forms.Label();
            this.label_Azimuth = new System.Windows.Forms.Label();
            this.label_Altitude = new System.Windows.Forms.Label();
            this.label_Buttons = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
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
            this.label_PressureRawValue = new System.Windows.Forms.Label();
            this.label_PressureRaw = new System.Windows.Forms.Label();
            this.trackBar_BrushSize = new System.Windows.Forms.TrackBar();
            this.label_BrushSizeValue = new System.Windows.Forms.Label();
            this.label_BrushSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_BrushSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label_X
            // 
            this.label_X.AutoSize = true;
            this.label_X.Location = new System.Drawing.Point(23, 27);
            this.label_X.Name = "label_X";
            this.label_X.Size = new System.Drawing.Size(33, 32);
            this.label_X.TabIndex = 8;
            this.label_X.Text = "X";
            // 
            // label_Y
            // 
            this.label_Y.AutoSize = true;
            this.label_Y.Location = new System.Drawing.Point(23, 71);
            this.label_Y.Name = "label_Y";
            this.label_Y.Size = new System.Drawing.Size(33, 32);
            this.label_Y.TabIndex = 9;
            this.label_Y.Text = "Y";
            // 
            // label_Z
            // 
            this.label_Z.AutoSize = true;
            this.label_Z.Location = new System.Drawing.Point(23, 115);
            this.label_Z.Name = "label_Z";
            this.label_Z.Size = new System.Drawing.Size(31, 32);
            this.label_Z.TabIndex = 10;
            this.label_Z.Text = "Z";
            // 
            // label_Pressure
            // 
            this.label_Pressure.AutoSize = true;
            this.label_Pressure.Location = new System.Drawing.Point(23, 222);
            this.label_Pressure.Name = "label_Pressure";
            this.label_Pressure.Size = new System.Drawing.Size(127, 32);
            this.label_Pressure.TabIndex = 11;
            this.label_Pressure.Text = "Pressure";
            // 
            // label_Azimuth
            // 
            this.label_Azimuth.AutoSize = true;
            this.label_Azimuth.Location = new System.Drawing.Point(23, 375);
            this.label_Azimuth.Name = "label_Azimuth";
            this.label_Azimuth.Size = new System.Drawing.Size(117, 32);
            this.label_Azimuth.TabIndex = 14;
            this.label_Azimuth.Text = "Azimuth";
            // 
            // label_Altitude
            // 
            this.label_Altitude.AutoSize = true;
            this.label_Altitude.Location = new System.Drawing.Point(23, 331);
            this.label_Altitude.Name = "label_Altitude";
            this.label_Altitude.Size = new System.Drawing.Size(111, 32);
            this.label_Altitude.TabIndex = 13;
            this.label_Altitude.Text = "Altitude";
            // 
            // label_Buttons
            // 
            this.label_Buttons.AutoSize = true;
            this.label_Buttons.Location = new System.Drawing.Point(23, 422);
            this.label_Buttons.Name = "label_Buttons";
            this.label_Buttons.Size = new System.Drawing.Size(111, 32);
            this.label_Buttons.TabIndex = 16;
            this.label_Buttons.Text = "Buttons";
            // 
            // label_Device
            // 
            this.label_Device.AutoSize = true;
            this.label_Device.Location = new System.Drawing.Point(23, 607);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(101, 32);
            this.label_Device.TabIndex = 18;
            this.label_Device.Text = "Device";
            // 
            // button_ClearCanvas
            // 
            this.button_ClearCanvas.Location = new System.Drawing.Point(1573, 717);
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
            this.pictureBox_Canvas.Location = new System.Drawing.Point(513, 71);
            this.pictureBox_Canvas.Name = "pictureBox_Canvas";
            this.pictureBox_Canvas.Size = new System.Drawing.Size(1280, 640);
            this.pictureBox_Canvas.TabIndex = 21;
            this.pictureBox_Canvas.TabStop = false;
            // 
            // label_AltitudeValue
            // 
            this.label_AltitudeValue.AutoSize = true;
            this.label_AltitudeValue.Location = new System.Drawing.Point(190, 331);
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
            this.label_PosZValue.Location = new System.Drawing.Point(190, 115);
            this.label_PosZValue.Name = "label_PosZValue";
            this.label_PosZValue.Size = new System.Drawing.Size(41, 32);
            this.label_PosZValue.TabIndex = 24;
            this.label_PosZValue.Text = "---";
            // 
            // label_PressureValue
            // 
            this.label_PressureValue.AutoSize = true;
            this.label_PressureValue.Location = new System.Drawing.Point(190, 222);
            this.label_PressureValue.Name = "label_PressureValue";
            this.label_PressureValue.Size = new System.Drawing.Size(41, 32);
            this.label_PressureValue.TabIndex = 25;
            this.label_PressureValue.Text = "---";
            // 
            // label_AzimuthValue
            // 
            this.label_AzimuthValue.AutoSize = true;
            this.label_AzimuthValue.Location = new System.Drawing.Point(190, 375);
            this.label_AzimuthValue.Name = "label_AzimuthValue";
            this.label_AzimuthValue.Size = new System.Drawing.Size(41, 32);
            this.label_AzimuthValue.TabIndex = 27;
            this.label_AzimuthValue.Text = "---";
            // 
            // label_ButtonsValue
            // 
            this.label_ButtonsValue.AutoSize = true;
            this.label_ButtonsValue.Location = new System.Drawing.Point(190, 422);
            this.label_ButtonsValue.Name = "label_ButtonsValue";
            this.label_ButtonsValue.Size = new System.Drawing.Size(41, 32);
            this.label_ButtonsValue.TabIndex = 28;
            this.label_ButtonsValue.Text = "---";
            // 
            // label_DeviceValue
            // 
            this.label_DeviceValue.AutoSize = true;
            this.label_DeviceValue.Location = new System.Drawing.Point(190, 607);
            this.label_DeviceValue.Name = "label_DeviceValue";
            this.label_DeviceValue.Size = new System.Drawing.Size(41, 32);
            this.label_DeviceValue.TabIndex = 29;
            this.label_DeviceValue.Text = "---";
            // 
            // label_PressureRawValue
            // 
            this.label_PressureRawValue.AutoSize = true;
            this.label_PressureRawValue.Location = new System.Drawing.Point(190, 179);
            this.label_PressureRawValue.Name = "label_PressureRawValue";
            this.label_PressureRawValue.Size = new System.Drawing.Size(41, 32);
            this.label_PressureRawValue.TabIndex = 31;
            this.label_PressureRawValue.Text = "---";
            // 
            // label_PressureRaw
            // 
            this.label_PressureRaw.AutoSize = true;
            this.label_PressureRaw.Location = new System.Drawing.Point(23, 179);
            this.label_PressureRaw.Name = "label_PressureRaw";
            this.label_PressureRaw.Size = new System.Drawing.Size(127, 32);
            this.label_PressureRaw.TabIndex = 30;
            this.label_PressureRaw.Text = "Pressure";
            // 
            // trackBar_BrushSize
            // 
            this.trackBar_BrushSize.AutoSize = false;
            this.trackBar_BrushSize.Location = new System.Drawing.Point(659, 14);
            this.trackBar_BrushSize.Maximum = 100;
            this.trackBar_BrushSize.Minimum = 1;
            this.trackBar_BrushSize.Name = "trackBar_BrushSize";
            this.trackBar_BrushSize.Size = new System.Drawing.Size(424, 45);
            this.trackBar_BrushSize.TabIndex = 32;
            this.trackBar_BrushSize.Value = 1;
            this.trackBar_BrushSize.Scroll += new System.EventHandler(this.trackBar_BrushSize_Scroll);
            // 
            // label_BrushSizeValue
            // 
            this.label_BrushSizeValue.AutoSize = true;
            this.label_BrushSizeValue.Location = new System.Drawing.Point(1089, 21);
            this.label_BrushSizeValue.Name = "label_BrushSizeValue";
            this.label_BrushSizeValue.Size = new System.Drawing.Size(41, 32);
            this.label_BrushSizeValue.TabIndex = 33;
            this.label_BrushSizeValue.Text = "---";
            // 
            // label_BrushSize
            // 
            this.label_BrushSize.AutoSize = true;
            this.label_BrushSize.Location = new System.Drawing.Point(507, 14);
            this.label_BrushSize.Name = "label_BrushSize";
            this.label_BrushSize.Size = new System.Drawing.Size(146, 32);
            this.label_BrushSize.TabIndex = 34;
            this.label_BrushSize.Text = "Brush size";
            // 
            // FormApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1805, 786);
            this.Controls.Add(this.label_BrushSize);
            this.Controls.Add(this.label_BrushSizeValue);
            this.Controls.Add(this.trackBar_BrushSize);
            this.Controls.Add(this.label_PressureRawValue);
            this.Controls.Add(this.label_PressureRaw);
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
            this.Controls.Add(this.label_Altitude);
            this.Controls.Add(this.label_X);
            this.Controls.Add(this.label_Y);
            this.Controls.Add(this.label_Z);
            this.Controls.Add(this.label_Pressure);
            this.Controls.Add(this.label_Azimuth);
            this.Controls.Add(this.label_Device);
            this.Controls.Add(this.label_Buttons);
            this.Name = "FormApp";
            this.Text = "WinTab Painter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_BrushSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_X;
        private System.Windows.Forms.Label label_Y;
        private System.Windows.Forms.Label label_Z;
        private System.Windows.Forms.Label label_Pressure;
        private System.Windows.Forms.Label label_Azimuth;
        private System.Windows.Forms.Label label_Altitude;
        private System.Windows.Forms.Label label_Buttons;
        private System.Windows.Forms.Label label_Device;
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
        private System.Windows.Forms.Label label_PressureRawValue;
        private System.Windows.Forms.Label label_PressureRaw;
        private System.Windows.Forms.TrackBar trackBar_BrushSize;
        private System.Windows.Forms.Label label_BrushSizeValue;
        private System.Windows.Forms.Label label_BrushSize;
    }
}

